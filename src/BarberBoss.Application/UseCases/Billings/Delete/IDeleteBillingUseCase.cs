namespace BarberBoss.Application.UseCases.Billings.Delete;

public interface IDeleteBillingUseCase
{
    Task Execute(long id);
}
