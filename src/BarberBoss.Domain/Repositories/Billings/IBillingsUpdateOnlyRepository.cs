using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingsUpdateOnlyRepository
{
    Task<Billing?> GetById(long id);
    void Update(Billing billing);
}
