using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;


namespace Restaurants.API.Middlerwares.Tests;

public class ErrorHandlingMiddlerwareTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddlerware>> _loggerMock;
    private readonly ErrorHandlingMiddlerware _middlerware;
    public ErrorHandlingMiddlerwareTests()
    {
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddlerware>>();
        _middlerware = new ErrorHandlingMiddlerware(_loggerMock.Object);
    }
    [Fact]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        //arrange
        var context = new DefaultHttpContext();
        var nextDelegateMock=new Mock<RequestDelegate>();
        //act
        await _middlerware.InvokeAsync(context, nextDelegateMock.Object);
        //assert
        nextDelegateMock.Verify(r=>r.Invoke(context), Times.Once());
    }
    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        //arrange
        var context = new DefaultHttpContext();
        var exception = new NotFoundException(nameof(Restaurant), "1");
        //act
        await _middlerware.InvokeAsync(context, _ => throw exception);
        //assert
        context.Response.StatusCode.Should().Be(404);
    }
    [Fact]
    public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
    {
        //arrange
        var context = new DefaultHttpContext();
        var exception = new ForbidException();
        //act
        await _middlerware.InvokeAsync(context, _ => throw exception);
        //assert
        context.Response.StatusCode.Should().Be(403);
    }
    [Fact]
    public async Task InvokeAsync_WhenGenericThrown_ShouldSetStatusCode500()
    {
        //arrange
        var context = new DefaultHttpContext();
        var exception = new Exception();
        //act
        await _middlerware.InvokeAsync(context, _ => throw exception);
        //assert
        context.Response.StatusCode.Should().Be(500);
    }

}