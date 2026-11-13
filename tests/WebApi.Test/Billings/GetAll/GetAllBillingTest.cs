using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Billings.GetAll;

public class GetAllBillingTest : BarberBossClassFixture
{
    private const string METHOD = "api/billings";

    private readonly string _token;

    private readonly string _tokenWithoutBillings;

    public GetAllBillingTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Barber.GetToken();
        _tokenWithoutBillings = webApplicationFactory.User_WithoutBillings.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: METHOD, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("billings").EnumerateArray().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var result = await DoGet(requestUri: METHOD, token: _tokenWithoutBillings);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
