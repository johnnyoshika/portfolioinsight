using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PortfolioInsight
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<CurrencyEntity> Currencies { get; set; }
        public DbSet<ListingExchangeEntity> ListingExchanges { get; set; }
        public DbSet<SymbolEntity> Symbols { get; set; }
        public DbSet<BrokerageEntity> Brokerages { get; set; }
        public DbSet<BrokerageSymbolEntity> BrokerageSymbols { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AuthorizationEntity> Authorizations { get; set; }
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<BalanceEntity> Balances { get; set; }
        public DbSet<PositionEntity> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyEntity>(entity =>
            {
                entity.ToTable("Currencies");

                entity.HasKey(c => c.Code);

                entity.Property(c => c.Code)
                    .HasMaxLength(3);

                entity.HasData(
                    new CurrencyEntity { Code = "CAD" },
                    new CurrencyEntity { Code = "USA" }
                );
            });

            modelBuilder.Entity<ListingExchangeEntity>(entity =>
            {
                entity.ToTable("ListingExchanges");

                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code)
                    .HasMaxLength(10);

                entity.HasData(
                    new ListingExchangeEntity { Code = "TSX" },
                    new ListingExchangeEntity { Code = "TSXV" },
                    new ListingExchangeEntity { Code = "CNSX" },
                    new ListingExchangeEntity { Code = "MX" },
                    new ListingExchangeEntity { Code = "NASDAQ" },
                    new ListingExchangeEntity { Code = "NYSE" },
                    new ListingExchangeEntity { Code = "NYSEAM" },
                    new ListingExchangeEntity { Code = "ARCA" },
                    new ListingExchangeEntity { Code = "OPRA" },
                    new ListingExchangeEntity { Code = "PinkSheets" },
                    new ListingExchangeEntity { Code = "OTCBB" }
                );
            });

            modelBuilder.Entity<SymbolEntity>(entity =>
            {
                entity.ToTable("Symbols");

                entity.Property(s => s.Name)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(s => s.CurrencyCode)
                    .HasMaxLength(3)
                    .IsRequired();

                entity.HasOne(s => s.Currency)
                    .WithMany()
                    .HasForeignKey(s => s.CurrencyCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(s => s.ListingExchangeCode)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.HasOne(s => s.ListingExchange)
                    .WithMany(e => e.Symbols)
                    .HasForeignKey(s => s.ListingExchangeCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new { s.Name, s.ListingExchangeCode})
                    .IsUnique();
            });

            modelBuilder.Entity<BrokerageEntity>(entity =>
            {
                entity.ToTable("Brokerages");

                entity.Property(b => b.Id)
                    .ValueGeneratedNever();

                entity.Property(b => b.Name)
                    .IsRequired();

                entity.HasData(new BrokerageEntity { Id = 1, Name = "Questrade" });
            });

            modelBuilder.Entity<BrokerageSymbolEntity>(entity =>
            {
                entity.ToTable("BrokerageSymbols");

                entity.HasKey(bs => new { bs.SymbolId, bs.BrokerageId });

                entity.Property(bs => bs.ReferenceId)
                    .IsRequired();

                entity.HasIndex(bs => new { bs.ReferenceId, bs.BrokerageId })
                    .IsUnique();

                entity.HasOne(bs => bs.Symbol)
                    .WithMany(s => s.BrokerageSymbols)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(bs => bs.Brokerage)
                    .WithMany(s => s.BrokerageSymbols)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(u => u.FirstName)
                    .IsRequired();

                entity.Property(u => u.LastName)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .IsRequired();

                entity.Property(u => u.PasswordHash)
                    .HasMaxLength(48);
            });

            modelBuilder.Entity<AuthorizationEntity>(entity =>
            {
                entity.ToTable("Authorizations");

                entity.HasIndex(a => new { a.BrokerageId, a.BrokerageUserId, a.UserId })
                    .IsUnique();

                entity.Property(a => a.BrokerageUserId)
                    .IsRequired();

                entity.Property(a => a.RefreshToken)
                    .IsRequired();

                entity.HasOne(a => a.Brokerage)
                        .WithMany(b => b.Authorizations)
                        .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.User)
                        .WithMany(u => u.Authorizations)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AccountEntity>(entity =>
            {
                entity.ToTable("Accounts");

                entity.Property(a => a.Number)
                    .IsRequired();

                entity.Property(a => a.Name)
                    .IsRequired();

                entity.HasIndex(a => new { a.Number, a.AuthorizationId })
                    .IsUnique();

                entity.HasOne(a => a.Authorization)
                    .WithMany(a => a.Accounts)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BalanceEntity>(entity =>
            {
                entity.ToTable("Balances");

                entity.Property(b => b.Value)
                    .HasColumnType("money");

                entity.Property(b => b.Type)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(b => b.CurrencyCode)
                    .HasMaxLength(3)
                    .IsRequired();

                entity.HasIndex(b => new { b.Type, b.CurrencyCode, b.AccountId })
                    .IsUnique();

                entity.HasOne(b => b.Currency)
                    .WithMany()
                    .HasForeignKey(b => b.CurrencyCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Account)
                    .WithMany(b => b.Balances)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PositionEntity>(entity =>
            {
                entity.ToTable("Positions");

                entity.Property(p => p.Value)
                    .HasColumnType("money");

                entity.HasIndex(p => new { p.SymbolId, p.AccountId })
                    .IsUnique();

                entity.HasOne(p => p.Symbol)
                    .WithMany(s => s.Positions)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }

    public partial class CurrencyEntity
    {
        public string Code { get; set; }
    }

    public partial class ListingExchangeEntity
    {
        public string Code { get; set; }

        public List<SymbolEntity> Symbols { get; set; }
    }

    public partial class SymbolEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string CurrencyCode { get; set; }
        public CurrencyEntity Currency { get; set; }

        public string ListingExchangeCode { get; set; }
        public ListingExchangeEntity ListingExchange { get; set; }

        public List<BrokerageSymbolEntity> BrokerageSymbols { get; set; }
        public List<PositionEntity> Positions { get; set; }
    }

    public partial class BrokerageEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<BrokerageSymbolEntity> BrokerageSymbols { get; set; }
        public List<AuthorizationEntity> Authorizations { get; set; }
    }

    public partial class BrokerageSymbolEntity
    {
        public int SymbolId { get; set; }
        public SymbolEntity Symbol { get; set; }

        public int BrokerageId { get; set; }
        public BrokerageEntity Brokerage { get; set; }

        public string ReferenceId { get; set; }
    }

    public partial class UserEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public int ActivityCount { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int LoginCount { get; set; }

        public List<AuthorizationEntity> Authorizations { get; set; }
    }

    public partial class AuthorizationEntity
    {
        public int Id { get; set; }

        public int BrokerageId { get; set; }
        public BrokerageEntity Brokerage { get; set; }

        public string BrokerageUserId { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public string RefreshToken { get; set; }

        public List<AccountEntity> Accounts { get; set; }
    }

    public partial class AccountEntity
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }

        public int AuthorizationId { get; set; }
        public AuthorizationEntity Authorization { get; set; }

        public List<BalanceEntity> Balances { get; set; }
        public List<PositionEntity> Positions { get; set; }
    }

    public partial class BalanceEntity
    {
        public int Id { get; set; }
        public decimal Value { get; set; }

        public string Type { get; set; }

        public string CurrencyCode { get; set; }
        public CurrencyEntity Currency { get; set; }

        public int AccountId { get; set; }
        public AccountEntity Account { get; set; }
    }

    public partial class PositionEntity
    {
        public int Id { get; set; }
        public decimal Value { get; set; }

        public int SymbolId { get; set; }
        public SymbolEntity Symbol { get; set; }

        public int AccountId { get; set; }
        public AccountEntity Account { get; set; }
    }
}
