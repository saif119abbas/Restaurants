using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Restaurants.Domain.Constatnts;
using FluentAssertions;

namespace Restaurants.Applications.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticated_ShouldReturnCurrentUser()
        {
            //arrnage
            var dateOfBirth = new DateOnly(1990, 1, 1);
            var httpContextAccessorMock=new Mock<IHttpContextAccessor>();
            var claims = new List<Claim> 
            {
                new (ClaimTypes.NameIdentifier,"1"),
                new (ClaimTypes.Email,"test2@gmail.com"),
                new (ClaimTypes.Role,UserRoles.Admin),
                new (ClaimTypes.Role,UserRoles.User),
                new ("Nationality","Palestinian"),
                new ("DateOfBirth",dateOfBirth.ToString("yyyy-MM-dd"))
            };
            var user=new ClaimsPrincipal(new ClaimsIdentity(claims,"Test"));
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User=user
            });

            var userContext = new UserContext(httpContextAccessorMock.Object);
            //act
            var currentUser = userContext.GetCurrentUser();
            //ASSERT
            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test2@gmail.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin,UserRoles.User);
            currentUser.Nationality.Should().Be("Palestinian");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);

        }
        [Fact()]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvlaidOperationException()
        {
            //arrnage
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null!);
            var userContext = new UserContext(httpContextAccessorMock.Object);

            //act
            Action action = () => userContext.GetCurrentUser();
            //assert
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("User context is not present");
        }
        [Fact()]
        public void GetCurrentUser_WithNullIdentity_ShouldReturnNull()
        {
            //arrnage
            var dateOfBirth = new DateOnly(1990, 1, 1);
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var userContext = new UserContext(httpContextAccessorMock.Object);
            var user = new ClaimsPrincipal();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });
            //act
           var currentUser= userContext.GetCurrentUser();
            //assert
            currentUser.Should().BeNull();
        }
    }
}
