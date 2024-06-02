using Microsoft.EntityFrameworkCore;
using BuberBreakfast.Data;
using BuberBreakfast.Models;
using dotenv.net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

DotEnv.Load();
var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<BreakfastDbContext>()
        .AddDefaultTokenProviders();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

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