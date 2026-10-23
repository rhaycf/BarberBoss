
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using ClosedXML.Excel;

namespace BarberBoss.Application.UseCases.Billings.Reports.Excel;

public class BillingsReportExcelUseCase : IBillingsReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private readonly IBillingsReadOnlyRepository _repository;

    public BillingsReportExcelUseCase(IBillingsReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly startDate, DateOnly endDate)
    {
        var request = new RequestBillingReportJson
        {
            StartDate = startDate,
            EndDate = endDate
        };

        Validate(request);

        var billings = (await _repository.FilterByPeriod(startDate, endDate))
            .Where(billing => billing.Status != Status.Cancelled)
            .ToList();

        if (billings.Count == 0)
            return [];

        using var workbook = new XLWorkbook();

        workbook.Author = "Rhayane";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        var worksheetName = $"{startDate:dd-MM-yyyy} a {endDate:dd-MM-yyyy}";
        var worksheet = workbook.Worksheets.Add(worksheetName);

        InsertHeader(worksheet);

        var row = 2;
        foreach (var billing in billings)
        {
            worksheet.Cell($"A{row}").Value = billing.ServiceName;
            worksheet.Cell($"B{row}").Value = billing.Date;
            worksheet.Cell($"C{row}").Value = billing.PaymentMethod.PaymentMethodToString();

            worksheet.Cell($"D{row}").Value = billing.Amount;
            worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"{CURRENCY_SYMBOL} #,##0.00";

            worksheet.Cell($"E{row}").Value = billing.Notes;

            row++;
        }

        var lastRow = row - 1;
        worksheet.Range($"A2:A{lastRow}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        worksheet.Range($"B2:B{lastRow}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Range($"C2:C{lastRow}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Range($"D2:D{lastRow}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Range($"E2:E{lastRow}").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        // ajustar tamanho da coluna
        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.SERVICE_NAME;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_METHOD;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.NOTES;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;

        worksheet.Cells("A1:E1").Style.Font.FontColor = XLColor.FromHtml("#ffffff");
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#205858");

        worksheet.Range("A1:E1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    }

    private void Validate(RequestBillingReportJson request)
    {
        var validator = new BillingReportValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors
            .Select(e => e.ErrorMessage)
            .ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
