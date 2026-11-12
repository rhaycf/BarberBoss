using BarberBoss.Application.UseCases.Billings.Update;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Billings.Update;

public class UpdateBillingUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestBillingJsonBuilder.Build();
        var billing = BillingBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, billing);

        var act = async () => await useCase.Execute(billing.Id, request);

        await act.Should().NotThrowAsync();

        billing.Date.Should().Be(request.Date);
        billing.BarberName.Should().Be(request.BarberName);
        billing.ClientName.Should().Be(request.ClientName);
        billing.ServiceName.Should().Be(request.ServiceName);
        billing.Amount.Should().Be(request.Amount);
        billing.PaymentMethod.Should().Be((PaymentMethod)request.PaymentMethod);
        billing.Status.Should().Be((Status)request.Status);
        billing.Notes.Should().Be(request.Notes);
        billing.CreatedAt.Should().Be(request.CreatedAt);
        billing.UpdatedAt.Should().Be(request.UpdatedAt);
    }

    [Fact]
    public async Task Error_ServiceName_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var billing = BillingBuilder.Build(loggedUser);

        var request = RequestBillingJsonBuilder.Build();
        request.ServiceName = string.Empty;

        var useCase = CreateUseCase(loggedUser, billing);

        var act = async () => await useCase.Execute(billing.Id, request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.SERVICE_NAME_REQUIRED_AND_INVALID_LENGTH));
    }

    [Fact]
    public async Task Error_Billing_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var request = RequestBillingJsonBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000, request);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BILLING_NOT_FOUND));
    }

    private UpdateBillingUseCase CreateUseCase(User user, Billing? billing = null)
    {
        var repository = new BillingsUpdateOnlyRepositoryBuilder().GetById(user, billing).Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateBillingUseCase(mapper, unitOfWork, repository, loggedUser);
    }
}
