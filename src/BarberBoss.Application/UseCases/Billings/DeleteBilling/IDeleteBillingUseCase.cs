namespace BarberBoss.Application.UseCases.Billings.DeleteBilling;

public interface IDeleteBillingUseCase
{
    Task Execute(long id);
}
