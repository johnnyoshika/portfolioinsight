# Setup
* Clone this repo
* Copy `appsettings.json` and name it `appsettings.development.json`
* Set default connection string in `appsettings.development.json`
* Set connection string in [ContextFactory](https://github.com/johnnyoshika/portfolioinsight/blob/master/src/PortfolioInsight.Sql/ContextFactory.cs)
  * This connectoin string will be used by EF Migration
* To create database, from Package Manager Console:
  * Run:`update-database`
* Manually create a user in the database, as there's no client for this. Use [PasswordSecurity_Hash_Should](https://github.com/johnnyoshika/portfolioinsight/blob/master/test/PortfolioInsight.Domain.Tests/Security/PasswordSecurity_Hash_Should.cs) to generate a password.

# Questrade Setup
* Create an app at [App Hub](https://login.questrade.com/APIAccess/UserApps.aspx)
* Add this callback URL. Note: it seems that callback URLs must be https and it cannot be localhost.
  * `https://app.portfolioinsight.local:44348/questrade/response`
* Get Consumer Key from App Hub and set `QuestradeConsumerKey` in `appsettings.development.json`

# Questrade Authorization (enable app.portfolioinsight.local)
* In `.vs/config/applicationhost.config`
* Find this:
```
        <bindings>
          <binding protocol="http" bindingInformation="*:53837:localhost" />
          <binding protocol="https" bindingInformation="*:44348:localhost" />
        </bindings>
```
* Add an extra binding for `app.propertyinsight.local`
```
        <bindings>
          <binding protocol="http" bindingInformation="*:53837:localhost" />
          <binding protocol="https" bindingInformation="*:44348:localhost" />
          <binding protocol="https" bindingInformation="*:44348:app.portfolioinsight.local" />
        </bindings>
```
* The moment you add this `app.portfolioinsight.local` binding, you need to run Visual Studio as an administrator, otherwise, IIS Express won't run. That's a bit of a pain, so you may want to only add that binding for Questrade authorization, then remove it thereafter.
* Add this entry in your host file:
```
        127.0.0.1		app.portfolioinsight.local
```
* Go through the OAuth flow using https://app.portfolioinsight.local:44348

# Questrade Authorization (w/o administrator mode)
* The above Questrade Authorization option has the downside of having to run Visual Studio as an administrator. To avoid this, use this workaround.
* Delete `<binding protocol="https" bindingInformation="*:44348:app.portfolioinsight.local" />` binding entry in applicationhost.config if it was added
* Modify [ApplicationUrl's](https://github.com/johnnyoshika/portfolioinsight/blob/master/src/PortfolioInsight.Web/Http/ApplicationUrl.cs) AbsoluteHost() to return the host URL entered as a callback URL in App Hub (e.g. if the callback URL is `https://app.portfolioinsight.local:44348/questrade/response`, then AbsoluteHost() should return `https://app.portfolioinsight.local:44348`)
* After authorization and after user is redirected back to `https://app.portfolioinsight.local:44348/questrade/response`, change the host URL to our localhost URL so that the URL looks like this: `https://localhost:44348/questrade/response?code={code}`. Load that page
* OAuth flow will continue as expected
* Revert changes to `ApplicationUrl.AbsoluteHost()`

# Questrade Authorization (last resort)
Sometimes Questrade's authorization API stops working. See email I sent to `apisupport@questrade.com` titled `Is Questrade's API authorization working?`. In such a case:
* Go to Questrade's App Hub
* Manually generate a new token
* Manually save the generated refresh token in the `Connections` database table
  * Hint: See `Accounts` table understand which `Connections`' record needs to be updated

# PortfolioInsight.Questrade.Tests
* In order to run tests in `PortfolioInsight.Questrade.Tests`, you must first create a file in `PortfolioInsight.Questrade.Tests/keys` directory with filename `token.txt` with the content being a manually generate a refresh token from [https://login.questrade.com/APIAccess/UserApps.aspx](https://login.questrade.com/APIAccess/UserApps.aspx).

# EF Migration
* After making changes to entities, open the Package Manager Console and:
  * Add Migration: `add-migration <migration name> -StartupProject PortfolioInsight.Sql -Project PortfolioInsight.Sql`
  * Update Database: `update-database -StartupProject PortfolioInsight.Sql -Project PortfolioInsight.Sql`
* `ContextFactory` is used to create DbContext with connection string injection when running these commands in Package Manager Console. Alternatively, we could have set the `ConnectionString` environment variable in Package Manager Console prior to running the commands above: `$env:ConnectionString='Server=.;Database=PortfolioInsight;...'`