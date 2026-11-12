using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Billings.Update;

public class UpdateBillingTest : BarberBossClassFixture
{
    private const string METHOD = "api/billings";

    private readonly string _token;
    private readonly long _billingId;

    public UpdateBillingTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Barber.GetToken();
        _billingId = webApplicationFactory.Billing_Barber.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestBillingJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_billingId}", request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_ServiceName_Empty(string culture)
    {
        var request = RequestBillingJsonBuilder.Build();
        request.ServiceName = string.Empty;

        var result = await DoPut(requestUri: $"{METHOD}/{_billingId}", request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("SERVICE_NAME_REQUIRED_AND_INVALID_LENGTH", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Billing_Not_Found(string culture)
    {
        var request = RequestBillingJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("BILLING_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
