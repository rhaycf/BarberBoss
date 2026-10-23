using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billings.RegisterBilling;

public interface IRegisterBillingUseCase
{
    Task<ResponseRegisteredBillingJson> Execute(RequestBillingJson request);
}
