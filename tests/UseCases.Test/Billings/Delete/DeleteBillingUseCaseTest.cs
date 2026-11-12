using BarberBoss.Application.UseCases.Billings.Delete;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Billings.Delete;

public class DeleteBillingUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var billing = BillingBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, billing);

        var act = async () => await useCase.Execute(billing.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Billing_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BILLING_NOT_FOUND));
    }

    private DeleteBillingUseCase CreateUseCase(User user, Billing? billing = null)
    {
        var repository = new BillingsReadOnlyRepositoryBuilder().GetById(user, billing).Build();
        var repositoryWriteOnly = BillingsWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteBillingUseCase(repository, repositoryWriteOnly, unitOfWork, loggedUser);
    }
}
