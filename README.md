# Setup

- Clone this repo
- Copy `appsettings.json` and name it `appsettings.development.json`
- Set default connection string in `appsettings.development.json` to `Data Source=PortfolioInsight.sqlite`
- Create a folder called `portfolioinsight-data` as a sibling to this repo
- Set connection string in [ContextFactory](https://github.com/johnnyoshika/portfolioinsight/blob/main/src/PortfolioInsight.Sql/ContextFactory.cs)
  - This connection string will be used by EF Migration if we update database from Package Manager Console
- To create database from Package Manager Console (note: this is optional, as automatic migrations will do this on startup):
  - Run:`update-database`
- Manually create a user in the database, as there's no client for this. Use [PasswordSecurity_Hash_Should](https://github.com/johnnyoshika/portfolioinsight/blob/main/test/PortfolioInsight.Domain.Tests/Security/PasswordSecurity_Hash_Should.cs) to generate a password.

# Questrade Setup

- Create an app at [App Hub](https://login.questrade.com/APIAccess/UserApps.aspx)
- Add the following callback URLs. Note: it seems that callback URLs must be https and it cannot be localhost.
  - `https://app.portfolioinsight.local:44348/questrade/response`
  - Any other URL where authorization is required (e.g. Cloudflare Tunnel URL on Raspberry Pi)
- Get Consumer Key from App Hub and set `QuestradeConsumerKey` in `appsettings.development.json`

# PortfolioInsight.Questrade.Tests

- In order to run tests in `PortfolioInsight.Questrade.Tests`, you must first create a file in `PortfolioInsight.Questrade.Tests/keys` directory with filename `token.txt` with the content being a manually generate a refresh token from [https://login.questrade.com/APIAccess/UserApps.aspx](https://login.questrade.com/APIAccess/UserApps.aspx).

# EF Migration

- After making changes to entities, open the Package Manager Console and:
  - Add Migration: `add-migration <migration name> -StartupProject PortfolioInsight.Sql -Project PortfolioInsight.Sql`
  - Update Database (optional, as automatic migrations is set up): `update-database -StartupProject PortfolioInsight.Sql -Project PortfolioInsight.Sql`
- `ContextFactory` is used to create DbContext with connection string injection when running these commands in Package Manager Console. Alternatively, we could have set the `ConnectionString` environment variable in Package Manager Console prior to running the commands above: `$env:ConnectionString='{redacted}'`
