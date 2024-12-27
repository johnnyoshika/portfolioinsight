# Setup

- Copy `appsettings.json` and name it `appsettings.development.json`
- Set default connection string in `appsettings.development.json` to `Data Source=PortfolioInsight.sqlite`
- Run:

```
dotnet restore
dotnet build
dotnet run --project src/PortfolioInsight.Web
```

- Set connection string in [ContextFactory](https://github.com/johnnyoshika/portfolioinsight/blob/main/src/PortfolioInsight.Sql/ContextFactory.cs)
  - This connection string will be used by EF Migration if we update database from Package Manager Console
- To create database from Package Manager Console (note: this is optional, as automatic migrations will do this on startup):
  - Run:`update-database`

## Development and Production

- Manually create a user in the database, as there's no client for this. Use [PasswordSecurity_Hash_Should](https://github.com/johnnyoshika/portfolioinsight/blob/main/test/PortfolioInsight.Domain.Tests/Security/PasswordSecurity_Hash_Should.cs) to generate a password.

# Questrade Setup

- Create an app at [App Hub](https://login.questrade.com/APIAccess/UserApps.aspx)
- Add the following callback URLs. Note: callback URLs must be https and it cannot be localhost.
  - `https://app.portfolioinsight.local:44348/questrade/response` (for development)
  - `https://portfolioinsight.oshika.com/questrade/response` (for production)
- Get Consumer Key from App Hub and set `QuestradeConsumerKey` in `appsettings.development.json`

# PortfolioInsight.Questrade.Tests

- In order to run tests in `PortfolioInsight.Questrade.Tests`, you must first create a file in `PortfolioInsight.Questrade.Tests/keys` directory with filename `token.txt` with the content being a manually generate a refresh token from [https://login.questrade.com/APIAccess/UserApps.aspx](https://login.questrade.com/APIAccess/UserApps.aspx).

# EF Migration

- After making changes to entities, open the Package Manager Console and:
  - Add Migration: `add-migration <migration name> -StartupProject PortfolioInsight.Sql -Project PortfolioInsight.Sql`
  - Update Database (optional, as automatic migrations is set up): `update-database -StartupProject PortfolioInsight.Sql -Project PortfolioInsight.Sql`
- `ContextFactory` is used to create DbContext with connection string injection when running these commands in Package Manager Console. Alternatively, we could have set the `ConnectionString` environment variable in Package Manager Console prior to running the commands above: `$env:ConnectionString='{redacted}'`

# Deployment

Refer to [PortfolioInsight on Pi](https://docs.google.com/document/d/1xS6VdkAlF7i67e3akkh3Dl6_vonzcmqAng2yJKYf17Y/edit)

# Update Refresh Tokens

Questrade's refresh tokens are single-use only - each refresh token must be stored and updated when exchanging it for a new access token.

If we get an error when syncing portfolios, there's a good chance that the Questrade refresh tokens expired. To update them:

- Ensure QuestradeConsumerKey in appsettings.development json (development) or appsettings.Production.json (production) matches the key for the user that we want to update refresh tokens for
- Sign in to Questrade under that user
- Go to https://app.portfolioinsight.local:44348/questrade/request (development) or https://portfolioinsight.oshika.com/questrade/request (production). This will redirect you to Questrade to authorize the app.
- After the redirect, adjust the URL if necessary (e.g. change app.portfolioinsight.local:44348 to portfolioinsight.oshika.com if doing this in production)
