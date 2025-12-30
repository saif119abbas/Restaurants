using Restaurants.Infastructure.Extensions;
using Restaurants.Infastructure.Seedrs;
using Serilog;
using Serilog.Formatting.Compact;
using Restaurants.API.Middlerwares;
using Restaurants.Domain.Entities;
using Restaurants.API.Extensions;
using Restaurants.Applications.Extensions;
try
{

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddApplication();
    builder.AddPresentaion();
    builder.Services.AddInfastructure(builder.Configuration);
    new CompactJsonFormatter();
    var app = builder.Build();
    var scope = app.Services.CreateScope();
    var seeder= scope.ServiceProvider.GetRequiredService<IRestaruntSeeder>();
    await seeder.Seed();
    app.UseMiddleware<ErrorHandlingMiddlerware>();
    app.UseMiddleware<RequestTimeLoggingMiddleware>();
    app.UseSerilogRequestLogging();
    if(app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.MapGroup("api/identity")
        .WithTags("Identity")
        .MapIdentityApi<User>();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}
public partial class Program{ }
