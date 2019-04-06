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
    }
}
