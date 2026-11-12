using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;

namespace BarberBoss.Application.UseCases.Billings.Delete;

public class DeleteBillingUseCase : IDeleteBillingUseCase
{
    private readonly IBillingsReadOnlyRepository _billingsReadOnlyRepository;
    private readonly IBillingsWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteBillingUseCase(IBillingsReadOnlyRepository billingsReadOnlyRepository,
                                IBillingsWriteOnlyRepository repository, 
                                IUnitOfWork unitOfWork,
                                ILoggedUser loggedUser)
    {
        _billingsReadOnlyRepository = billingsReadOnlyRepository;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var billing = await _billingsReadOnlyRepository.GetById(loggedUser, id);

        if (billing is null)
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);

        await _repository.Delete(id);

        await _unitOfWork.Commit();
    }
}
