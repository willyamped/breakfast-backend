var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IBreakfastService, BreakfastService>();
}


var app = builder.Build();
{
    app.UseExceptionHandler("/error"); //global error handling
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}