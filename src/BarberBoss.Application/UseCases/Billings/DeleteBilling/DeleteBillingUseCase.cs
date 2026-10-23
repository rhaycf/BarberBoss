using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;

namespace BarberBoss.Application.UseCases.Billings.DeleteBilling;

public class DeleteBillingUseCase : IDeleteBillingUseCase
{
    private readonly IBillingsWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBillingUseCase(IBillingsWriteOnlyRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var result = await _repository.Delete(id);

        if (result == false)
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);

        await _unitOfWork.Commit();
    }
}
