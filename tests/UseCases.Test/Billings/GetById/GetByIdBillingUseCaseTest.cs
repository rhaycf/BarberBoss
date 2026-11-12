using BarberBoss.Application.UseCases.Billings.GetById;
using BarberBoss.Communication.Enums;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using DocumentFormat.OpenXml.Bibliography;
using FluentAssertions;
using PdfSharp.Drawing;

namespace UseCases.Test.Billings.GetById;

public class GetByIdBillingUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var billing = BillingBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, billing);

        var result = await useCase.Execute(billing.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(billing.Id);
        result.Date.Should().Be(billing.Date);
        result.BarberName.Should().Be(billing.BarberName);
        result.ClientName.Should().Be(billing.ClientName);
        result.ServiceName.Should().Be(billing.ServiceName);
        result.Amount.Should().Be(billing.Amount);
        result.PaymentMethod.Should().Be((PaymentMethod)billing.PaymentMethod);
        result.Status.Should().Be((Status)billing.Status);
        result.Notes.Should().Be(billing.Notes);
        result.CreatedAt.Should().Be(billing.CreatedAt);
        result.UpdatedAt.Should().Be(billing.UpdatedAt);
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

    private GetByIdBillingUseCase CreateUseCase(User user, Billing? billing = null)
    {
        var repository = new BillingsReadOnlyRepositoryBuilder().GetById(user, billing).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetByIdBillingUseCase(repository, mapper, loggedUser);
    }
}
