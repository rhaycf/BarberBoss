using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Billings.Register;

public class RegisterBillingTest : BarberBossClassFixture
{
    private const string METHOD = "api/billings";

    private readonly string _token;

    public RegisterBillingTest(CustomWebApplicationFactory customWebApplication) : base(customWebApplication)
    {
        _token = customWebApplication.User_Barber.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestBillingJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("serviceName").GetString().Should().Be(request.ServiceName);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_ServiceName_Empty(string culture)
    {
        var request = RequestBillingJsonBuilder.Build();
        request.ServiceName = string.Empty;

        var result = await DoPost(requestUri: METHOD, request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("SERVICE_NAME_REQUIRED_AND_INVALID_LENGTH", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
