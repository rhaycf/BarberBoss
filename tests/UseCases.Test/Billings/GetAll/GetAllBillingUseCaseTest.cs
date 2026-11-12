using BarberBoss.Application.UseCases.Billings.GetAll;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Billings.GetAll;

public class GetAllBillingUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var billings = BillingBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, billings);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Billings.Should().NotBeNullOrEmpty().And.AllSatisfy(billing =>
        {
            billing.Id.Should().BeGreaterThan(0);
            billing.Date.Should().NotBe(default);
            billing.BarberName.Should().NotBeNullOrEmpty();
            billing.ClientName.Should().NotBeNullOrEmpty();
            billing.ServiceName.Should().NotBeNullOrEmpty();
            billing.Amount.Should().BeGreaterThan(0);
        });
    }

    private GetAllBillingUseCase CreateUseCase(User user, List<Billing> billings)
    {
        var repository = new BillingsReadOnlyRepositoryBuilder().GetAll(user, billings).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAllBillingUseCase(repository, mapper, loggedUser);
    }
}
