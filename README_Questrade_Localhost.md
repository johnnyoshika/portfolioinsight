When running on localhost, follow these instructions to authorize Questrade.

# Questrade Authorization (enable app.portfolioinsight.local)

- In `.vs/config/applicationhost.config`
- Find this:

```
        <bindings>
          <binding protocol="http" bindingInformation="*:53837:localhost" />
          <binding protocol="https" bindingInformation="*:44348:localhost" />
        </bindings>
```

- Add an extra binding for `app.propertyinsight.local`

```
        <bindings>
          <binding protocol="http" bindingInformation="*:53837:localhost" />
          <binding protocol="https" bindingInformation="*:44348:localhost" />
          <binding protocol="https" bindingInformation="*:44348:app.portfolioinsight.local" />
        </bindings>
```

- The moment you add this `app.portfolioinsight.local` binding, you need to run Visual Studio as an administrator, otherwise, IIS Express won't run. That's a bit of a pain, so you may want to only add that binding for Questrade authorization, then remove it thereafter.
- Add this entry in your host file:

```
        127.0.0.1		app.portfolioinsight.local
```

- Go through the OAuth flow using https://app.portfolioinsight.local:44348

# Questrade Authorization (w/o administrator mode)

- The above Questrade Authorization option has the downside of having to run Visual Studio as an administrator. To avoid this, use this workaround.
- Delete `<binding protocol="https" bindingInformation="*:44348:app.portfolioinsight.local" />` binding entry in applicationhost.config if it was added
- Modify [ApplicationUrl's](https://github.com/johnnyoshika/portfolioinsight/blob/main/src/PortfolioInsight.Web/Http/ApplicationUrl.cs) AbsoluteHost() to return the host URL entered as a callback URL in App Hub (e.g. if the callback URL is `https://app.portfolioinsight.local:44348/questrade/response`, then AbsoluteHost() should return `https://app.portfolioinsight.local:44348`)
- After authorization and after user is redirected back to `https://app.portfolioinsight.local:44348/questrade/response`, change the host URL to our localhost URL so that the URL looks like this: `https://localhost:44348/questrade/response?code={code}`. Load that page
- OAuth flow will continue as expected
- Revert changes to `ApplicationUrl.AbsoluteHost()`

# Questrade Authorization (last resort)

Sometimes Questrade's authorization API stops working. See email I sent to `apisupport@questrade.com` titled `Is Questrade's API authorization working?`. In such a case:

- Go to Questrade's App Hub
- Manually generate a new token
- Manually save the generated refresh token in the `Connections` database table
  - Hint: See `Accounts` table understand which `Connections`' record needs to be updated
