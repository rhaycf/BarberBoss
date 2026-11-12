using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Services.LoggedUser;

namespace BarberBoss.Application.UseCases.Billings.GetAll;

public class GetAllBillingUseCase : IGetAllBillingUseCase
{
    private readonly IBillingsReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetAllBillingUseCase(IBillingsReadOnlyRepository repository, 
                                IMapper mapper,
                                ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseBillingsJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _repository.GetAll(loggedUser);

        return new ResponseBillingsJson
        {
            Billings = _mapper.Map<List<ResponseShortBillingJson>>(result)
        };
    }
}
