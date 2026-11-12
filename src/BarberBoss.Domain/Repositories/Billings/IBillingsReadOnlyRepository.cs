using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingsReadOnlyRepository
{
    Task<List<Billing>> GetAll(Entities.User user);
    Task<Billing?> GetById(Entities.User user, long id);
    Task<List<Billing>> FilterByPeriod(Entities.User user, DateOnly startDate, DateOnly endDate);
}
