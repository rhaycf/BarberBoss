using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Application.UseCases.Billings.GetAllBillings;

public class GetAllBillingUseCase : IGetAllBillingUseCase
{
    private readonly IBillingsReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetAllBillingUseCase(IBillingsReadOnlyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseBillingsJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseBillingsJson
        {
            Billings = _mapper.Map<List<ResponseShortBillingJson>>(result)
        };
    }
}
