using Serilog;
using Restaurants.API.Middlerwares;
using Microsoft.OpenApi.Models;


namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddPresentaion(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="bearerAuth"
                }
            },
           []
        }
    });
        });
        builder.Services.AddControllers();

        builder.Services.AddScoped<ErrorHandlingMiddlerware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();
        builder.Host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration)
        );
    }
}
