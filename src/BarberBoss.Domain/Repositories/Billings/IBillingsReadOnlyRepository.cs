using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingsReadOnlyRepository
{
    Task<List<Billing>> GetAll();
    Task<Billing?> GetById(long id);
    Task<List<Billing>> FilterByPeriod(DateOnly startDate, DateOnly endDate);
}
