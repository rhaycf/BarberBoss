using BarberBoss.Domain.Entities;

namespace WebApi.Test.Resources;

public class BillingIdentityManager
{
    private readonly Billing _billing;

    public BillingIdentityManager(Billing billing)
    {
        _billing = billing;
    }

    public long GetId() => _billing.Id;

    public DateTime GetDate() => _billing.Date;
}
