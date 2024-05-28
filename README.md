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

dotnet ef migrations add InitialCreate

dotnet ef database update
```