using FluentValidation.TestHelper;
using System.Xml.Linq;
using Xunit;

namespace Restaurants.Applications.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void Validator_ForValidCommand_ShouldNotHaveValidationError()
    {
        //arrnage
        var command = new CreateRestaurantCommand()
        {
            Name = "Test",
            Description = "This a test description",
            Category = "Italian",
            ContactEmail="test@test.com",
            ContactNumber= "+972598765432",
            City="Nablus",
            HasDelivery=true,
            PostalCode="10-010",
            Street="123, MainStreet"
        };
        var validator = new CreateRestaurantCommandValidator();
        //act
        var result=validator.TestValidate(command);

        //assert
        result.ShouldNotHaveAnyValidationErrors();


    }
    [Fact()]
    public void Validator_ForInValidCommand_ShouldHaveValidationError()
    {
        //arrnage
        var command = new CreateRestaurantCommand()
        {
            Name = "Te",
            Description = "1",
            Category = "Italianssss",
            ContactEmail = "@test.com",
            ContactNumber = "+1 415 555 267",
            City = "Nablus",
            HasDelivery = true,
            PostalCode = "55555",
            Street = "123, MainStreet"
        };
        var validator = new CreateRestaurantCommandValidator();
        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
        result.ShouldHaveValidationErrorFor(c => c.ContactNumber);
    }
    [Theory()]
    //["Italian", "Mexican", "Japanese", "American", "Indian"];
    [InlineData("Italian")]
    [InlineData("Japanese")]
    [InlineData("Indian")]
    [InlineData("American")]
    [InlineData("Mexican")]

    public void Validator_ForValidCategory_ShouldNotHaveValidationErrorForCategoryProperty(string category)
    {
        //arrnage
        var command = new CreateRestaurantCommand()
        {
            Category = category,
        };
        var validator = new CreateRestaurantCommandValidator();
        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldNotHaveValidationErrorFor(c=>c.Category);


    }
    [Theory()]
    //["Italian", "Mexican", "Japanese", "American", "Indian"];
    [InlineData("12345")]
    [InlineData("123-45")]
    [InlineData("12 345")]
    [InlineData("123 45")]
    [InlineData("12 -345")]
    [InlineData("12-3 45")]

    public void Validator_ForValidPostalCode_ShouldHaveValidationErrorForPostalCodeProperty(string postalCode)
    {
        //arrnage
        var command = new CreateRestaurantCommand()
        {
            PostalCode = postalCode,
        };
        var validator = new CreateRestaurantCommandValidator();
        //act
        var result = validator.TestValidate(command);

        //assert
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);


    }
}