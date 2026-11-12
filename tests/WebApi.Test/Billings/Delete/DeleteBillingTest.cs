using BarberBoss.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Billings.Delete;

public class DeleteBillingTest : BarberBossClassFixture
{
    private const string METHOD = "api/billings";

    private readonly string _token;
    private readonly long _billingId;

    public DeleteBillingTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Barber.GetToken();
        _billingId = webApplicationFactory.Billing_Barber.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_billingId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{METHOD}/{_billingId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Billing_Not_Found(string culture)
    {
        var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("BILLING_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
