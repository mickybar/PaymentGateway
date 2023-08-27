# PaymentGateway

Simple implementation of a Payment gateway solution

## Design choices and assumptions

- The solution has been written using .NET 6 (current LTS version); tests are written using xUnit
- Card data is not encrypted at rest and in transit; this is ok given the scope of the exercise, but it should never happen in a production environment
- No security is added to the API; as for the previous point, that's something that should absolutely avoided in a production environment
- For the data layer, EF Core with a code first approach is used; SQLite has been used for its simplicity; in a real environment, depending on the storage/performance requirements, it could be another relational database (SQL Server, MySQL) or a NoSQL one (CosmosDB, MongoDB, Cassandra etc.)
- The solution can be containerised and deployed into the cloud (i.e. AppServices or AKS)
- No async suffix has been added to async methods; that's a deliberate choice

## Area for improvements

- Adding encryption
- Adding security
- Containerise the solution
- Increase code coverage: only the main controller has been covered with unit tests, given the quite simple logic of every other component

## Running the solution

- The assumption is to have .NET 6 sdk installed with the dotnet CLI available
- Clone the solution in your local drive
- Open a console in /src folder
- run `dotnet ef migrations add PaymentsDbInitialMigration --startup-project PaymentGateway.Api --project PaymentGateway.Data`
- run `dotnet ef database update --startup-project C:\code\c#\PaymentGateway\src\PaymentGateway.Api\PaymentGateway.Api.csproj`
- start the solution
- if no automatic browser is opening, browse to: `https://localhost:7151/swagger/index.html`
- Rely on Swagger documentation to test the API endpoints
