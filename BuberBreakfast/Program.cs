using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BuberBreakfast.Data;
using dotenv.net;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();

    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    builder.Services.AddDbContext<BreakfastDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection")
    .Replace("{DB_USER}", dbUser)
    .Replace("{DB_PASSWORD}", dbPassword),
    new MySqlServerVersion(new Version(7, 0, 0))));
}


var app = builder.Build();
{
    app.UseExceptionHandler("/error"); //global error handling
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}