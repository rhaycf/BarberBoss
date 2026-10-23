using BarberBoss.Communication.Requests;

namespace BarberBoss.Application.UseCases.Billings.UpdateBilling;

public interface IUpdateBillingUseCase
{
    Task Execute(long id, RequestBillingJson request);
}
