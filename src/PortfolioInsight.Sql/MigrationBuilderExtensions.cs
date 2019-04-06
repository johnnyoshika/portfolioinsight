using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight
{
    public static class MigrationBuilderExtensions
    {
        public static void SetCurrencyCodeCollationToCaseSensitive(this MigrationBuilder builder) =>
            // To be run during Initial_Create migration
            // https://github.com/aspnet/EntityFrameworkCore/issues/6577#issuecomment-452883425
            builder.Sql(@"
                ALTER TABLE [dbo].[Positions] DROP CONSTRAINT [FK_Positions_Currencies_CurrencyCode]

                ALTER TABLE [dbo].[Currencies] DROP CONSTRAINT [PK_Currencies] WITH ( ONLINE = OFF )

                ALTER TABLE [dbo].[Currencies]
                ALTER COLUMN [Code] [nvarchar](3)
                COLLATE Latin1_General_CS_AS NOT NULL

                ALTER TABLE [dbo].[Positions]
                ALTER COLUMN [CurrencyCode] [nvarchar](3)
                COLLATE Latin1_General_CS_AS NOT NULL

                ALTER TABLE [dbo].[Currencies] ADD  CONSTRAINT [PK_Currencies] PRIMARY KEY CLUSTERED 
                (
	                [Code] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

                ALTER TABLE [dbo].[Positions]  WITH CHECK ADD  CONSTRAINT [FK_Positions_Currencies_CurrencyCode] FOREIGN KEY([CurrencyCode])
                REFERENCES [dbo].[Currencies] ([Code])
                ALTER TABLE [dbo].[Positions] CHECK CONSTRAINT [FK_Positions_Currencies_CurrencyCode]
            ", true);

        public static void SetBalancesCurrencyCodeCollationToCaseSensitity(this MigrationBuilder builder) =>
            builder.Sql(@"
                ALTER TABLE [dbo].[Balances]
                ALTER COLUMN [CurrencyCode] [nvarchar](3)
                COLLATE Latin1_General_CS_AS NOT NULL
            ");

        public static void SetSymbolsCurrencyCodeCollationToCaseSensitive(this MigrationBuilder builder) =>
            builder.Sql(@"
                ALTER TABLE [dbo].[Symbols]
                ALTER COLUMN [CurrencyCode] [nvarchar](3)
                COLLATE Latin1_General_CS_AS NOT NULL
            ");

        public static void SetListingExchangeCodeCollationToCaseSensitive(this MigrationBuilder builder) =>
            builder.Sql(@"
                ALTER TABLE [dbo].[ListingExchanges] DROP CONSTRAINT [PK_ListingExchanges] WITH ( ONLINE = OFF )

                ALTER TABLE [dbo].[ListingExchanges]
                ALTER COLUMN [Code] [nvarchar](10)
                COLLATE Latin1_General_CS_AS NOT NULL

                ALTER TABLE [dbo].[Symbols]
                ALTER COLUMN [ListingExchangeCode] [nvarchar](10)
                COLLATE Latin1_General_CS_AS NOT NULL

                ALTER TABLE [dbo].[ListingExchanges] ADD  CONSTRAINT [PK_ListingExchanges] PRIMARY KEY CLUSTERED 
                (
	                [Code] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ");

        public static void BeforeSymbolsListingExchangeCodeChange(this MigrationBuilder builder) =>
            builder.Sql(@"
                DROP INDEX [IX_Symbols_ListingExchangeCode] ON [dbo].[Symbols]
                ALTER TABLE [dbo].[Symbols] DROP CONSTRAINT [FK_Symbols_ListingExchanges_ListingExchangeCode]
            ");

        public static void AfterSymbolsListingExchangeCodeChange(this MigrationBuilder builder) =>
            builder.Sql(@"
                ALTER TABLE [dbo].[Symbols]  WITH CHECK ADD  CONSTRAINT [FK_Symbols_ListingExchanges_ListingExchangeCode] FOREIGN KEY([ListingExchangeCode])
                REFERENCES [dbo].[ListingExchanges] ([Code])
                ALTER TABLE [dbo].[Symbols] CHECK CONSTRAINT [FK_Symbols_ListingExchanges_ListingExchangeCode]

                CREATE NONCLUSTERED INDEX [IX_Symbols_ListingExchangeCode] ON [dbo].[Symbols]
                (
	                [ListingExchangeCode] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ");

        public static void SetSymbolsListingExchangeCodeNullCollationToCaseSensitive(this MigrationBuilder builder) =>
            builder.Sql(@"
                        ALTER TABLE [dbo].[Symbols]
                        ALTER COLUMN [ListingExchangeCode] [nvarchar](10)
                        COLLATE Latin1_General_CS_AS
                    ");

        public static void SetSymbolsListingExchangeCodeNotNullCollationToCaseSensitive(this MigrationBuilder builder) =>
            builder.Sql(@"
                ALTER TABLE [dbo].[Symbols]
                ALTER COLUMN [ListingExchangeCode] [nvarchar](10)
                COLLATE Latin1_General_CS_AS NOT NULL
            ");
    }
}
