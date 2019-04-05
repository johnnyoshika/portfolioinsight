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
* Add this callback URL. Note: it seems that callback URLs must be https and it cannot be localhost.
  * `https://app.portfolioinsight.local:44348/questrade/response`
* Get Consumer Key from App Hub and set `QuestradeConsumerKey` in `appsettings.json`

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
* Modify [ApplicationUrl's](https://github.com/johnnyoshika/portfolioinsight/blob/master/src/PortfolioInsight.Web/Http/ApplicationUrl.cs) AbsoluteHost() to return the host URL entered as a callback URL in App Hub (e.g. `https://app.portfolioinsight.local:44348/questrade/response`)
* After authorization and after user is redirected back to `https://app.portfolioinsight.local:44348/questrade/response`, change the host URL to our localhost URL so that the URL looks like this: `https://localhost:44348/questrade/response?code={code}`. Load that page
* OAuth flow will continue as expected

# EF Migration
* After making changes to entities, open the Package Manager Console and:
  * Set the default project to `src\PortfolioInsight.Sql`
  * Run: `add-migration <migration name>`
  * Run: `update-database`
* `ContextFactory` is used to create DbContext with connection string injection when running these commands in Package Manager Console