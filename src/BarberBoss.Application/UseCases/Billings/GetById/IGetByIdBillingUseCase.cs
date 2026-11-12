using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billings.GetById;

public interface IGetByIdBillingUseCase
{
    Task<ResponseBillingJson> Execute(long id);
}
