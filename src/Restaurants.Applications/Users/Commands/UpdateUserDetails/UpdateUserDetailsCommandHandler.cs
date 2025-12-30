
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Applications.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailsCommandHandler(
    ILogger<UpdateUserDetailsCommandHandler> logger,
    IUserStore<User> userStore,
    IUserContext userContext
    ) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        if(user == null) //new
        {
            throw new UnauthorizedException();
        }
        logger.LogInformation("Updating user:{UserId}, with {@Request}", user.Id, request);
        var dbUser = await userStore.FindByIdAsync(user.Id, cancellationToken);
        if(dbUser==null)
        {
            throw new NotFoundException(nameof(User), user.Id);
        }
        dbUser.DateOfBirth = request.DateOfBirth;
        dbUser.Nationality=request.Nationality;
        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}
