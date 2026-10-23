using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;

namespace BarberBoss.Application.UseCases.Billings.GetById;

public class GetByIdBillingUseCase : IGetByIdBillingUseCase
{
    private readonly IBillingsReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetByIdBillingUseCase(IBillingsReadOnlyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseBillingJson> Execute(long id)
    {
        var result = await _repository.GetById(id);

        if (result is null)
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);

        return _mapper.Map<ResponseBillingJson>(result);
    }
}
