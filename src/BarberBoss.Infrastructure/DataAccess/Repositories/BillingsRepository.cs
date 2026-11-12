using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class BillingsRepository : IBillingsReadOnlyRepository, IBillingsWriteOnlyRepository, IBillingsUpdateOnlyRepository
{
    private readonly BarberBossDbContext _dbContext;

    public BillingsRepository(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Billing billing)
    {
        await _dbContext.Billings.AddAsync(billing);
    }

    public async Task<List<Billing>> GetAll(User user)
    {
        return await _dbContext.Billings.AsNoTracking().Where(biling => biling.UserId == user.Id).ToListAsync();
    }

    async Task<Billing?> IBillingsReadOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.Billings.FirstOrDefaultAsync(billing => billing.Id == id && billing.UserId == user.Id);
    }

    async Task<Billing?> IBillingsUpdateOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.Billings.FirstOrDefaultAsync(billing => billing.Id == id && billing.UserId == user.Id);
    }

    public void Update(Billing billing)
    {
        _dbContext.Billings.Update(billing);
    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Billings.FindAsync(id);

        _dbContext.Billings.Remove(result!);
    }

    public async Task<List<Billing>> FilterByPeriod(User user, DateOnly startDate, DateOnly endDate)
    {
        var start = startDate.ToDateTime(TimeOnly.MinValue);
        var end = endDate.ToDateTime(TimeOnly.MaxValue);

        return await _dbContext
            .Billings
            .AsNoTracking()
            .Where(billing => billing.UserId == user.Id && billing.Date >= start && billing.Date <= end)
            .OrderBy(billing => billing.Date)
            .ThenBy(billing => billing.ServiceName)
            .ToListAsync();
    }
}
