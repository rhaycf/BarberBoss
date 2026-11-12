using FluentAssertions;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Billings.Reports;

public class BillingsReportTest : BarberBossClassFixture
{
    private const string METHOD = "api/report";

    private readonly string _adminToken;
    private readonly string _barberToken;
    private readonly DateTime _billingDate;

    public BillingsReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken();
        _barberToken = webApplicationFactory.User_Barber.GetToken();
        _billingDate = webApplicationFactory.Billing_Admin.GetDate();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var startDate = new DateOnly(_billingDate.Year, _billingDate.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var result = await DoGet(requestUri: $"{METHOD}/pdf?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Success_Excel()
    {
        var startDate = new DateOnly(_billingDate.Year, _billingDate.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var result = await DoGet(requestUri: $"{METHOD}/excel?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(
            requestUri: $"{METHOD}/pdf?startDate={_billingDate:yyyy-MM-dd}&endDate={_billingDate:yyyy-MM-dd}",
            token: _barberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(
            requestUri: $"{METHOD}/excel?startDate={_billingDate:yyyy-MM-dd}&endDate={_billingDate:yyyy-MM-dd}",
            token: _barberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
