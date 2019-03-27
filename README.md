# EF Migration
* After making changes to entities, open the Package Manager Console and:
  * Set the default project to `src\PortfolioInsight.Sql`
  * Run: `add-migration <migration name>`
  * Run `update-database`
* `ContextFactory` is used to create DbContext with connection string injection when running these commands in Package Manager Console