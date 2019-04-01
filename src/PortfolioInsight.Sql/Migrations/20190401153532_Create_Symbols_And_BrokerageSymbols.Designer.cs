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
    [Migration("20190401153532_Create_Symbols_And_BrokerageSymbols")]
    partial class Create_Symbols_And_BrokerageSymbols
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PortfolioInsight.AccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorizationId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Number")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AuthorizationId");

                    b.HasIndex("Number", "AuthorizationId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PortfolioInsight.AuthorizationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BrokerageId");

                    b.Property<string>("BrokerageUserId")
                        .IsRequired();

                    b.Property<string>("RefreshToken")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("BrokerageId", "BrokerageUserId", "UserId")
                        .IsUnique();

                    b.ToTable("Authorizations");
                });

            modelBuilder.Entity("PortfolioInsight.BrokerageEntity", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Brokerages");

                    b.HasData(
                        new { Id = 1, Name = "Questrade" }
                    );
                });

            modelBuilder.Entity("PortfolioInsight.BrokerageSymbolEntity", b =>
                {
                    b.Property<int>("SymbolId");

                    b.Property<int>("BrokerageId");

                    b.Property<string>("ReferenceId")
                        .IsRequired();

                    b.HasKey("SymbolId", "BrokerageId");

                    b.HasIndex("BrokerageId");

                    b.HasIndex("ReferenceId", "BrokerageId")
                        .IsUnique();

                    b.ToTable("BrokerageSymbols");
                });

            modelBuilder.Entity("PortfolioInsight.CurrencyEntity", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(3);

                    b.HasKey("Code");

                    b.ToTable("Currencies");

                    b.HasData(
                        new { Code = "CAD" },
                        new { Code = "USA" }
                    );
                });

            modelBuilder.Entity("PortfolioInsight.PositionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasMaxLength(3);

                    b.Property<int>("SymbolId");

                    b.Property<decimal>("Value")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CurrencyCode");

                    b.HasIndex("SymbolId", "AccountId")
                        .IsUnique();

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("PortfolioInsight.SymbolEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Symbols");
                });

            modelBuilder.Entity("PortfolioInsight.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ActivityCount");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<DateTime?>("LastActivityAt");

                    b.Property<DateTime?>("LastLoginAt");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<int>("LoginCount");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(48);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PortfolioInsight.AccountEntity", b =>
                {
                    b.HasOne("PortfolioInsight.AuthorizationEntity", "Authorization")
                        .WithMany("Accounts")
                        .HasForeignKey("AuthorizationId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PortfolioInsight.AuthorizationEntity", b =>
                {
                    b.HasOne("PortfolioInsight.BrokerageEntity", "Brokerage")
                        .WithMany("Authorizations")
                        .HasForeignKey("BrokerageId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PortfolioInsight.UserEntity", "User")
                        .WithMany("Authorizations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PortfolioInsight.BrokerageSymbolEntity", b =>
                {
                    b.HasOne("PortfolioInsight.BrokerageEntity", "Brokerage")
                        .WithMany("BrokerageSymbols")
                        .HasForeignKey("BrokerageId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PortfolioInsight.SymbolEntity", "Symbol")
                        .WithMany("BrokerageSymbols")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("PortfolioInsight.PositionEntity", b =>
                {
                    b.HasOne("PortfolioInsight.AccountEntity", "Account")
                        .WithMany("Positions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PortfolioInsight.CurrencyEntity", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyCode")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("PortfolioInsight.SymbolEntity", "Symbol")
                        .WithMany("Positions")
                        .HasForeignKey("SymbolId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
