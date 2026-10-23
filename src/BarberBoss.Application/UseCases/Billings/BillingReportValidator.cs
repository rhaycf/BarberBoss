using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Billings;

public class BillingReportValidator : AbstractValidator<RequestBillingReportJson>
{
    public BillingReportValidator()
    {
        RuleFor(request => request.StartDate)
            .LessThanOrEqualTo(request => request.EndDate)
            .WithMessage(
                ResourceErrorMessages.START_DATE_MUST_BE_LESS_THAN_OR_EQUAL_TO_END_DATE);
    }
}
