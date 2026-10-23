using BarberBoss.Domain.Repositories;

namespace BarberBoss.Infrastructure.DataAccess;

internal class UnitOfWork : IUnitOfWork
{
    private readonly BarberBossDbContext _billingDbContext;

    public UnitOfWork(BarberBossDbContext billingDbContext)
    {
        _billingDbContext = billingDbContext;
    }

    public async Task Commit() => await _billingDbContext.SaveChangesAsync();
}
