using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;

namespace BarberBoss.Application.UseCases.Billings.GetById;

public class GetByIdBillingUseCase : IGetByIdBillingUseCase
{
    private readonly IBillingsReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetByIdBillingUseCase(IBillingsReadOnlyRepository repository, 
                                 IMapper mapper,
                                 ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseBillingJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _repository.GetById(loggedUser, id);

        if (result is null)
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);

        return _mapper.Map<ResponseBillingJson>(result);
    }
}
