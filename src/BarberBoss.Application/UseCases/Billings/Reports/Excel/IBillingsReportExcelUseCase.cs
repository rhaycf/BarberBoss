namespace BarberBoss.Application.UseCases.Billings.Reports.Excel;

public interface IBillingsReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly startDate, DateOnly endDate);
}
