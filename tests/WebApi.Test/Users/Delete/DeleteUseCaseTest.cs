using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users.Delete;

public class DeleteUseCaseTest : BarberBossClassFixture
{
    private const string METHOD = "api/user";

    private readonly string _token;

    public DeleteUseCaseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Barber.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(METHOD, _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
