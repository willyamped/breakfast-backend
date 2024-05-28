 ```
 dotnet new sln -o BuberBreakfast 
 
 dotnet new classlib -o BuberBreakfast.contracts
 
 dotnet new webapi -o BuberBreakfast

 dotnet sln add $(find . -name '*.csproj')

 add contract reference to BB

 dotnet run --project ./BuberBreakfast
 ```   
