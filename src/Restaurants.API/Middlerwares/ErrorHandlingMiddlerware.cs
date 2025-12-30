
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlerwares;

public class ErrorHandlingMiddlerware(ILogger<ErrorHandlingMiddlerware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch(NotFoundException notFound)
        {
            logger.LogError(notFound, notFound.Message);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFound.Message);
        }
        catch (ForbidException )
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access forbidden");
        }
        catch(Exception ex)
        {

            logger.LogError(ex,ex.Message);
             context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong while processing the request");
        }
    }
}
