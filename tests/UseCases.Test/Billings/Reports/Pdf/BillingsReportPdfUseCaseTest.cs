using BarberBoss.Application.UseCases.Billings.Reports.Pdf;
using BarberBoss.Domain.Entities;
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

    private BillingsReportPdfUseCase CreateUseCase(User user, List<Billing> billings)
    {
        var repository = new BillingsReadOnlyRepositoryBuilder().FilterByPeriod(user, billings).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new BillingsReportPdfUseCase(repository, loggedUser);
    }
}
