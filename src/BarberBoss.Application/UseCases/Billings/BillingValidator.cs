using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Billings;

public class BillingValidator : AbstractValidator<RequestBillingJson>
{
    public BillingValidator()
    {
        RuleFor(billing => billing.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ResourceErrorMessages.DATE_CANNOT_BE_IN_THE_FUTURE);

        RuleFor(billing => billing.BarberName)
            .Must(name => !string.IsNullOrWhiteSpace(name) && name.Length is >= 2 and <= 80)
            .WithMessage(ResourceErrorMessages.BARBER_NAME_REQUIRED_AND_INVALID_LENGTH);
       
        RuleFor(billing => billing.ClientName)
            .Must(name => !string.IsNullOrWhiteSpace(name) && name.Length is >= 2 and <= 120)
            .WithMessage(ResourceErrorMessages.CLIENT_NAME_REQUIRED_AND_INVALID_LENGTH);

        RuleFor(billing => billing.ServiceName)
            .Must(name => !string.IsNullOrWhiteSpace(name) && name.Length is >= 2 and <= 120)
            .WithMessage(ResourceErrorMessages.SERVICE_NAME_REQUIRED_AND_INVALID_LENGTH);

        RuleFor(billing => billing.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_OR_EQUAL_TO_ZERO);

        RuleFor(billing => billing.Amount)
            .Equal(0)
            .When(billing => billing.Status == Communication.Enums.Status.Cancelled)
            .WithMessage(ResourceErrorMessages.CANCELLED_BILLING_AMOUNT_MUST_BE_ZERO);

        RuleFor(billing => billing.PaymentMethod)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.PAYMENT_METHOD_INVALID);

        RuleFor(billing => billing.Status)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.STATUS_INVALID);

        RuleFor(billing => billing.Notes)
            .Length(0, 500)
            .WithMessage(ResourceErrorMessages.NOTES_MAX_LENGTH_EXCEEDED);

        RuleFor(billing => billing.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ResourceErrorMessages.CREATED_AT_CANNOT_BE_IN_THE_FUTURE);

        RuleFor(billing => billing.UpdatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ResourceErrorMessages.UPDATED_AT_CANNOT_BE_IN_THE_FUTURE);
    }
}