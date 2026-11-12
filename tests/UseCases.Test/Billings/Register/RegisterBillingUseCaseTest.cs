using BarberBoss.Application.UseCases.Billings.Register;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Billings.Register;

public class RegisterBillingUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestBillingJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.ServiceName.Should().Be(request.ServiceName);
    }

    [Fact]
    public async Task Error_ServiceName_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var request = RequestBillingJsonBuilder.Build();
        request.ServiceName = string.Empty;

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.SERVICE_NAME_REQUIRED_AND_INVALID_LENGTH));
    }

    private RegisterBillingUseCase CreateUseCase(User user)
    {
        var repository = BillingsWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterBillingUseCase(repository, unitOfWork, mapper, loggedUser);
    }
}
