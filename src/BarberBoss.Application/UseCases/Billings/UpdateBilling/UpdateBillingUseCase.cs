using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Billings.UpdateBilling;

public class UpdateBillingUseCase : IUpdateBillingUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBillingsUpdateOnlyRepository _repository;

    public UpdateBillingUseCase(IMapper mapper, IUnitOfWork unitOfWork, IBillingsUpdateOnlyRepository repository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task Execute(long id, RequestBillingJson request)
    {
        Validate(request);

        var billing = await _repository.GetById(id);

        if (billing is null)
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);

        _mapper.Map(request, billing);

        _repository.Update(billing);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestBillingJson request)
    {
        var validator = new BillingValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
