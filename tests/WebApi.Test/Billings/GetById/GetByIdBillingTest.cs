using BarberBoss.Communication.Enums;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Billings.GetById;

public class GetByIdBillingTest : BarberBossClassFixture
{
    private const string METHOD = "api/billings";

    private readonly string _token;
    private readonly long _billingId;

    public GetByIdBillingTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Barber.GetToken();
        _billingId = webApplicationFactory.Billing_Barber.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_billingId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().Should().Be(_billingId);
        response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
        response.RootElement.GetProperty("barberName").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("clientName").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("serviceName").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);
        response.RootElement.GetProperty("notes").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("createdAt").GetDateTime().Should().NotBeAfter(DateTime.Today);
        response.RootElement.GetProperty("updatedAt").GetDateTime().Should().NotBeAfter(DateTime.Today);

        var paymentMethod = response.RootElement.GetProperty("paymentMethod").GetInt32();
        Enum.IsDefined(typeof(PaymentMethod), paymentMethod).Should().BeTrue();

        var status = response.RootElement.GetProperty("status").GetInt32();
        Enum.IsDefined(typeof(Status), status).Should().BeTrue();
    }
}
