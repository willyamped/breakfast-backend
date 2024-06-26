 ## Steps
 1. Run `dotnet restore` to install all packages
 
 ## List of Commands Used
 ```
 dotnet new sln -o BuberBreakfast 
 
 dotnet new classlib -o BuberBreakfast.contracts
 
 dotnet new webapi -o BuberBreakfast

 dotnet sln add $(find . -name '*.csproj')

 add contract reference to BB

 dotnet run --project ./BuberBreakfast
 ```


 ## Database   
```
dotnet add package Microsoft.EntityFrameworkCore

dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet ef migrations add <MIGRATION_NAME> e.g InitialCreate or UpdateDbContext

dotnet ef database update

mysql -u <USERNAME> -p <PASSWORD> -h <SERVER> -P <PORT_NUMBER> <DATABASE>
e.g mysql -u root -p -h localhost -P 3306 buberbreakfast
```