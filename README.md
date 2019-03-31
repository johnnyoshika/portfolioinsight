# Setup
* Clone this repo
* In `appsettings.json`, set default connection string
* Set connection string in [ContextFactory](https://github.com/johnnyoshika/portfolioinsight/blob/master/src/PortfolioInsight.Sql/ContextFactory.cs)
  * This connectoin string will be used by EF Migration
* To create database, from Package Manager Console:
  * Run:`update-database`
* Manually create a user in the database, as there's no client for this. Use [PasswordSecurity_Hash_Should](https://github.com/johnnyoshika/portfolioinsight/blob/master/test/PortfolioInsight.Domain.Tests/Security/PasswordSecurity_Hash_Should.cs) to generate a password.

# Questrade Setup
* Create an app at [App Hub](https://login.questrade.com/APIAccess/UserApps.aspx)
* Add a callback URL. It must be https and it cannot be localhost.
  * Unfortunately I couldn't get a custom base URL to work in Visual Studio, so use any https non-localhost URL, e.g. `https://johnny.netlify.com/questrade/response`
* Get Consumer Key from App Hub and set `QuestradeConsumerKey` in `appsettings.json`

# Questrade Authorization
* It is recommended above that a https non-localhost URL be used for the Callback URL (e.g. `https://johnny.netlify.com/questrade/response`)
* As a temporary workaround until Visual Studio supports [custom base URL with https](https://stackoverflow.com/q/55433429/188740), modify [ApplicationUrl's](https://github.com/johnnyoshika/portfolioinsight/blob/master/src/PortfolioInsight.Web/Http/ApplicationUrl.cs) AbsoluteHost() to return the host URL entered as a callback URL in App Hub (e.g. `https://johnny.netlify.com`)
* After authorization and after user is redirected back to `https://johnny.netlify.com/questrade/response`, change the host URL to our app URL so that the URL looks like this: `https://localhost:44348/questrade/response?code={code}`. Load that page
* OAuth flow will continue as expected

# EF Migration
* After making changes to entities, open the Package Manager Console and:
  * Set the default project to `src\PortfolioInsight.Sql`
  * Run: `add-migration <migration name>`
  * Run: `update-database`
* `ContextFactory` is used to create DbContext with connection string injection when running these commands in Package Manager Console