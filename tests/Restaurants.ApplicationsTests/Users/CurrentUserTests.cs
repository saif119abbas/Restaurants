using FluentAssertions;
using Restaurants.Domain.Constatnts;
using Xunit;

namespace Restaurants.Applications.Users.Tests;

public class CurrentUserTests
{
    //TestMethod_Scenario_ExpectedResult
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatcingRole_ShouldReturnTrue(string roleName)
    {
        //arrange
        var currentUser = new CurrentUser("1", "test2@gmail.com", [UserRoles.Admin, UserRoles.User], null, null);
        //act
        var isInRole=currentUser.IsInRole(roleName);
        //assert
        isInRole.Should().BeTrue();

    }
    [Fact()]
    public void IsInRole_WithNoMatcingRole_ShouldReturnFalse()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test2@gmail.com", [UserRoles.Admin, UserRoles.User], null, null);
        //act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);
        //assert
        isInRole.Should().BeFalse();

    }
    [Fact()]
    public void IsInRole_WithNoMatcingRoleCase_ShouldReturnFalse()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test2@gmail.com", [UserRoles.Admin, UserRoles.User], null, null);
        //act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());
        //assert
        isInRole.Should().BeFalse();

    }
}