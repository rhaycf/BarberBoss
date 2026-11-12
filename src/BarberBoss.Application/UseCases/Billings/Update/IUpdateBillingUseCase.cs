using BarberBoss.Communication.Requests;

namespace BarberBoss.Application.UseCases.Billings.Update;

public interface IUpdateBillingUseCase
{
    Task Execute(long id, RequestBillingJson request);
}
