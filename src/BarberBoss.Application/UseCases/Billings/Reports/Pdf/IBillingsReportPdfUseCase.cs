namespace BarberBoss.Application.UseCases.Billings.Reports.Pdf;

public interface IBillingsReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly startDate, DateOnly endDate);
}
