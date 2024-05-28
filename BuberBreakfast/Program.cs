using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BuberBreakfast.Data;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();
    builder.Services.AddDbContext<BreakfastDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(7, 0, 0))));
}


var app = builder.Build();
{
    app.UseExceptionHandler("/error"); //global error handling
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}