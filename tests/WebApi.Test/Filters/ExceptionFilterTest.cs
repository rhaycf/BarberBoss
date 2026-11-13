using BarberBoss.API.Filters;
using BarberBoss.Communication.Responses;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace WebApi.Test.Filters;

public class ExceptionFilterTest
{
    [Fact]
    public void Error_Unknown_Exception()
    {
        var context = CreateContext(new Exception("erro inesperado"));

        new ExceptionFilter().OnException(context);

        context.HttpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var result = context.Result.Should().BeOfType<ObjectResult>().Subject;
        var response = result.Value.Should().BeOfType<ResponseErrorJson>().Subject;

        response.ErrorMessages.Should().ContainSingle()
            .And.Contain(ResourceErrorMessages.UNKNOWN_ERROR);
    }

    [Fact]
    public void Error_BarberBoss_Exception()
    {
        var exception = new ErrorOnValidationException(new List<string> { "erro de validacao" });
        var context = CreateContext(exception);

        new ExceptionFilter().OnException(context);

        context.HttpContext.Response.StatusCode.Should().Be(exception.StatusCode);

        var result = context.Result.Should().BeOfType<ObjectResult>().Subject;
        var response = result.Value.Should().BeOfType<ResponseErrorJson>().Subject;

        response.ErrorMessages.Should().Contain("erro de validacao");
    }

    private static ExceptionContext CreateContext(Exception exception)
    {
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());

        return new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception
        };
    }
}
