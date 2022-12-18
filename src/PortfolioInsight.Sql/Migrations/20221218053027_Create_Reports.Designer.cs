﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortfolioInsight;

namespace PortfolioInsight.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20221218053027_Create_Reports")]
    partial class Create_Reports
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("PortfolioInsight.AccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ConnectionId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ConnectionId");

                    b.HasIndex("Number", "ConnectionId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PortfolioInsight.AllocationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("PortfolioId")
                        .HasColumnType("int");

                    b.Property<int>("SymbolId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SymbolId");

                    b.HasIndex("PortfolioId", "SymbolId")
                        .IsUnique();

                    b.ToTable("Allocations");
                });

            modelBuilder.Entity("PortfolioInsight.AllocationProportionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AllocationId")
                        .HasColumnType("int");

                    b.Property<int>("AssetClassId")
                        .HasColumnType("int");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AssetClassId");

                    b.HasIndex("AllocationId", "AssetClassId")
                        .IsUnique();

                    b.ToTable("AllocationProportions");
                });

            modelBuilder.Entity("PortfolioInsight.AssetClassEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PortfolioId")
                        .HasColumnType("int");

                    b.Property<decimal?>("Target")
                        .HasColumnType("decimal(4,3)");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioId");

                    b.HasIndex("Name", "PortfolioId")
                        .IsUnique();

                    b.ToTable("AssetClasses");
                });

            modelBuilder.Entity("PortfolioInsight.BalanceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal>("Value")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CurrencyCode");

                    b.HasIndex("Type", "CurrencyCode", "AccountId")
                        .IsUnique();

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("PortfolioInsight.BrokerageEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brokerages");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Questrade"
                        });
                });

            modelBuilder.Entity("PortfolioInsight.BrokerageSymbolEntity", b =>
                {
                    b.Property<int>("SymbolId")
                        .HasColumnType("int");

                    b.Property<int>("BrokerageId")
                        .HasColumnType("int");

                    b.Property<string>("ReferenceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SymbolId", "BrokerageId");

                    b.HasIndex("BrokerageId");

                    b.HasIndex("ReferenceId", "BrokerageId")
                        .IsUnique();

                    b.ToTable("BrokerageSymbols");
                });

            modelBuilder.Entity("PortfolioInsight.ConnectionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("BrokerageId")
                        .HasColumnType("int");

                    b.Property<string>("BrokerageUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("BrokerageId", "BrokerageUserId", "UserId")
                        .IsUnique();

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("PortfolioInsight.CurrencyEntity", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<DateTime>("AsOf")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(14,9)");

                    b.HasKey("Code");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            Code = "CAD",
                            AsOf = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Rate = 0m
                        },
                        new
                        {
                            Code = "USD",
                            AsOf = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Rate = 0m
                        });
                });

            modelBuilder.Entity("PortfolioInsight.ExcludeAccountEntity", b =>
                {
                    b.Property<int>("PortfolioId")
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.HasKey("PortfolioId", "AccountId");

                    b.HasIndex("AccountId");

                    b.ToTable("ExcludeAccounts");
                });

            modelBuilder.Entity("PortfolioInsight.ExcludeSymbolEntity", b =>
                {
                    b.Property<int>("PortfolioId")
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("SymbolId")
                        .HasColumnType("int");

                    b.HasKey("PortfolioId", "AccountId", "SymbolId");

                    b.HasIndex("AccountId");

                    b.HasIndex("SymbolId");

                    b.ToTable("ExcludeSymbols");
                });

            modelBuilder.Entity("PortfolioInsight.ListingExchangeEntity", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Code");

                    b.ToTable("ListingExchanges");

                    b.HasData(
                        new
                        {
                            Code = "TSX"
                        },
                        new
                        {
                            Code = "TSXV"
                        },
                        new
                        {
                            Code = "CNSX"
                        },
                        new
                        {
                            Code = "MX"
                        },
                        new
                        {
                            Code = "NASDAQ"
                        },
                        new
                        {
                            Code = "NYSE"
                        },
                        new
                        {
                            Code = "NYSEAM"
                        },
                        new
                        {
                            Code = "ARCA"
                        },
                        new
                        {
                            Code = "OPRA"
                        },
                        new
                        {
                            Code = "PinkSheets"
                        },
                        new
                        {
                            Code = "OTCBB"
                        });
                });

            modelBuilder.Entity("PortfolioInsight.PortfolioEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Portfolios");
                });

            modelBuilder.Entity("PortfolioInsight.PositionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("SymbolId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("SymbolId", "AccountId")
                        .IsUnique();

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("PortfolioInsight.ReportEntity", b =>
                {
                    b.Property<int>("PortfolioId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PortfolioId", "Date");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("PortfolioInsight.SymbolEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Description")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ListingExchangeCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyCode");

                    b.HasIndex("ListingExchangeCode");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Symbols");
                });

            modelBuilder.Entity("PortfolioInsight.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ActivityCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastActivityAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LoginCount")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(48)
                        .HasColumnType("nvarchar(48)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PortfolioInsight.AccountEntity", b =>
                {
                    b.HasOne("PortfolioInsight.ConnectionEntity", "Connection")
                        .WithMany("Accounts")
                        .HasForeignKey("ConnectionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Connection");
                });

            modelBuilder.Entity("PortfolioInsight.AllocationEntity", b =>
                {
                    b.HasOne("PortfolioInsight.PortfolioEntity", "Portfolio")
                        .WithMany("Allocations")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.SymbolEntity", "Symbol")
                        .WithMany()
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Portfolio");

                    b.Navigation("Symbol");
                });

            modelBuilder.Entity("PortfolioInsight.AllocationProportionEntity", b =>
                {
                    b.HasOne("PortfolioInsight.AllocationEntity", "Allocation")
                        .WithMany("Proportions")
                        .HasForeignKey("AllocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.AssetClassEntity", "AssetClass")
                        .WithMany()
                        .HasForeignKey("AssetClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Allocation");

                    b.Navigation("AssetClass");
                });

            modelBuilder.Entity("PortfolioInsight.AssetClassEntity", b =>
                {
                    b.HasOne("PortfolioInsight.PortfolioEntity", "Portfolio")
                        .WithMany("AssetClasses")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Portfolio");
                });

            modelBuilder.Entity("PortfolioInsight.BalanceEntity", b =>
                {
                    b.HasOne("PortfolioInsight.AccountEntity", "Account")
                        .WithMany("Balances")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.CurrencyEntity", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("PortfolioInsight.BrokerageSymbolEntity", b =>
                {
                    b.HasOne("PortfolioInsight.BrokerageEntity", "Brokerage")
                        .WithMany("BrokerageSymbols")
                        .HasForeignKey("BrokerageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.SymbolEntity", "Symbol")
                        .WithMany("BrokerageSymbols")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Brokerage");

                    b.Navigation("Symbol");
                });

            modelBuilder.Entity("PortfolioInsight.ConnectionEntity", b =>
                {
                    b.HasOne("PortfolioInsight.BrokerageEntity", "Brokerage")
                        .WithMany("Connections")
                        .HasForeignKey("BrokerageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.UserEntity", "User")
                        .WithMany("Connections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Brokerage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PortfolioInsight.ExcludeAccountEntity", b =>
                {
                    b.HasOne("PortfolioInsight.AccountEntity", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.PortfolioEntity", "Portfolio")
                        .WithMany()
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Portfolio");
                });

            modelBuilder.Entity("PortfolioInsight.ExcludeSymbolEntity", b =>
                {
                    b.HasOne("PortfolioInsight.AccountEntity", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.PortfolioEntity", "Portfolio")
                        .WithMany()
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.SymbolEntity", "Symbol")
                        .WithMany()
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Portfolio");

                    b.Navigation("Symbol");
                });

            modelBuilder.Entity("PortfolioInsight.PortfolioEntity", b =>
                {
                    b.HasOne("PortfolioInsight.UserEntity", "User")
                        .WithMany("Portfolios")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PortfolioInsight.PositionEntity", b =>
                {
                    b.HasOne("PortfolioInsight.AccountEntity", "Account")
                        .WithMany("Positions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.SymbolEntity", "Symbol")
                        .WithMany("Positions")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Symbol");
                });

            modelBuilder.Entity("PortfolioInsight.ReportEntity", b =>
                {
                    b.HasOne("PortfolioInsight.PortfolioEntity", "Portfolio")
                        .WithMany()
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");
                });

            modelBuilder.Entity("PortfolioInsight.SymbolEntity", b =>
                {
                    b.HasOne("PortfolioInsight.CurrencyEntity", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PortfolioInsight.ListingExchangeEntity", "ListingExchange")
                        .WithMany("Symbols")
                        .HasForeignKey("ListingExchangeCode")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Currency");

                    b.Navigation("ListingExchange");
                });

            modelBuilder.Entity("PortfolioInsight.AccountEntity", b =>
                {
                    b.Navigation("Balances");

                    b.Navigation("Positions");
                });

            modelBuilder.Entity("PortfolioInsight.AllocationEntity", b =>
                {
                    b.Navigation("Proportions");
                });

            modelBuilder.Entity("PortfolioInsight.BrokerageEntity", b =>
                {
                    b.Navigation("BrokerageSymbols");

                    b.Navigation("Connections");
                });

            modelBuilder.Entity("PortfolioInsight.ConnectionEntity", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("PortfolioInsight.ListingExchangeEntity", b =>
                {
                    b.Navigation("Symbols");
                });

            modelBuilder.Entity("PortfolioInsight.PortfolioEntity", b =>
                {
                    b.Navigation("Allocations");

                    b.Navigation("AssetClasses");
                });

            modelBuilder.Entity("PortfolioInsight.SymbolEntity", b =>
                {
                    b.Navigation("BrokerageSymbols");

                    b.Navigation("Positions");
                });

            modelBuilder.Entity("PortfolioInsight.UserEntity", b =>
                {
                    b.Navigation("Connections");

                    b.Navigation("Portfolios");
                });
#pragma warning restore 612, 618
        }
    }
}
