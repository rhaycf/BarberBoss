using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;
using Moq;

namespace CommonTestUtilities.Repositories;

public class BillingsReadOnlyRepositoryBuilder
{
    private readonly Mock<IBillingsReadOnlyRepository> _repository;

    public BillingsReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IBillingsReadOnlyRepository>();
    }

    public BillingsReadOnlyRepositoryBuilder GetAll(User user, List<Billing> billings)
    {
        _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(billings);

        return this;
    }

    public BillingsReadOnlyRepositoryBuilder GetById(User user, Billing? billing)
    {
        if (billing is not null)
            _repository.Setup(repository => repository.GetById(user, billing.Id)).ReturnsAsync(billing);

        return this;
    }

    public BillingsReadOnlyRepositoryBuilder FilterByPeriod(User user, List<Billing> billings)
    {
        _repository.Setup(repository => repository.FilterByPeriod(user, It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync(billings);

        return this;
    }

    public IBillingsReadOnlyRepository Build() => _repository.Object;
}
