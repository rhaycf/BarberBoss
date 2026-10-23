using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billings.GetAllBillings;

public interface IGetAllBillingUseCase
{
    Task<ResponseBillingsJson> Execute();
}
