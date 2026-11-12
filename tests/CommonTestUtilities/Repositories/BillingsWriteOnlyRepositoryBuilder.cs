using BarberBoss.Domain.Repositories.Billings;
using Moq;

namespace CommonTestUtilities.Repositories;

public class BillingsWriteOnlyRepositoryBuilder
{
    public static IBillingsWriteOnlyRepository Build()
    {
        var mock = new Mock<IBillingsWriteOnlyRepository>();

        return mock.Object;
    }
}
