using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Reports;

namespace BarberBoss.Domain.Extensions;

public static class PaymentMethodExtensions
{
    public static string PaymentMethodToString(this PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.Card => ResourceReportGenerationMessages.CARD,
            PaymentMethod.Cash => ResourceReportGenerationMessages.CASH,
            PaymentMethod.Pix => ResourceReportGenerationMessages.PIX,
            PaymentMethod.Other => ResourceReportGenerationMessages.OTHERS,
            _ => string.Empty
        };
    }
}
