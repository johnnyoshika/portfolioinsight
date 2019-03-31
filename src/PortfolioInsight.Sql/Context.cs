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
        public DbSet<BrokerageEntity> Brokerages { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AuthorizationEntity> Authorizations { get; set; }
        public DbSet<AccountEntity> Accounts { get; set; }
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

            modelBuilder.Entity<BrokerageEntity>(entity =>
            {
                entity.ToTable("Brokerages");

                entity.Property(b => b.Id)
                    .ValueGeneratedNever();

                entity.Property(b => b.Name)
                    .IsRequired();

                entity.HasData(new BrokerageEntity { Id = 1, Name = "Questrade" });
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

                entity
                    .HasIndex(a => new { a.BrokerageId, a.BrokerageUserId, a.UserId })
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

                entity
                    .HasIndex(a => new { a.Number, a.AuthorizationId })
                    .IsUnique();

                entity.HasOne(a => a.Authorization)
                        .WithMany(a => a.Accounts)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PositionEntity>(entity =>
            {
                entity.ToTable("Positions");

                entity.Property(p => p.Ticker)
                    .HasMaxLength(10)
                    .IsRequired();

                entity.Property(p => p.Value)
                    .HasColumnType("money");

                entity.Property(p => p.CurrencyCode)
                    .HasMaxLength(3)
                    .IsRequired();

                entity
                    .HasIndex(a => new { a.Ticker, a.AccountId })
                    .IsUnique();

                entity.HasOne(p => p.Currency)
                    .WithMany()
                    .HasForeignKey(p => p.CurrencyCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }

    public partial class CurrencyEntity
    {
        public string Code { get; set; }
    }

    public partial class BrokerageEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<AuthorizationEntity> Authorizations { get; set; }
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

        public List<PositionEntity> Positions { get; set; }
    }

    public partial class PositionEntity
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public decimal Value { get; set; }

        public string CurrencyCode { get; set; }
        public CurrencyEntity Currency { get; set; }

        public int AccountId { get; set; }
        public AccountEntity Account { get; set; }
    }
}
