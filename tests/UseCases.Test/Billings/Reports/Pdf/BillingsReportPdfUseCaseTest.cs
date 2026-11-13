using BarberBoss.Application.UseCases.Billings.Reports.Pdf;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Billings.Reports.Pdf;

public class BillingsReportPdfUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var billings = BillingBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, billings);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today));

        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser, []);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today));

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Error_StartDate_Greater_Than_EndDate()
    {
        var loggedUser = UserBuilder.Build();
        var billings = new List<Billing>();

        var useCase = CreateUseCase(loggedUser, billings);

        var startDate = DateOnly.FromDateTime(DateTime.Today);
        var endDate = startDate.AddDays(-1);

        Func<Task> act = async () => await useCase.Execute(startDate, endDate);

        await act.Should()
            .ThrowAsync<ErrorOnValidationException>();
    }

    private BillingsReportPdfUseCase CreateUseCase(User user, List<Billing> billings)
    {
        var repository = new BillingsReadOnlyRepositoryBuilder().FilterByPeriod(user, billings).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new BillingsReportPdfUseCase(repository, loggedUser);
    }
}
