
using BarberBoss.Application.UseCases.Billings.Reports.Pdf.Colors;
using BarberBoss.Application.UseCases.Billings.Reports.Pdf.Fonts;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace BarberBoss.Application.UseCases.Billings.Reports.Pdf;

public class BillingsReportPdfUseCase : IBillingsReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_BILLING_TABLE = 25;
    private readonly IBillingsReadOnlyRepository _repository;

    public BillingsReportPdfUseCase(IBillingsReadOnlyRepository repository)
    {
        _repository = repository;

        GlobalFontSettings.FontResolver = new BillingsReportFontResolver();
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

        var document = CreateDocument(startDate, endDate);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(page);

        var totalBillings = billings.Sum(b => b.Amount);

        CreateTotalSpentSection(page, startDate, endDate, totalBillings);

        foreach (var billing in billings)
        {
            var table = CreateBillingTable(page);

            // adiciona linha
            var row = table.AddRow();
            row.Height = HEIGHT_ROW_BILLING_TABLE;

            AddBillingServiceName(row.Cells[0], billing.ServiceName);
            AddHeaderForAmount(row.Cells[3]);

            // adiciona linha
            row = table.AddRow();
            row.Height = HEIGHT_ROW_BILLING_TABLE;

            // linha date
            row.Cells[0].AddParagraph(billing.Date.ToString("dd 'de' MMMM 'de' yyyy"));
            SetStyleBaseForBillingInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 10;

            // linha time
            row.Cells[1].AddParagraph(billing.Date.ToString("t"));
            SetStyleBaseForBillingInformation(row.Cells[1]);

            // linha paymentMethod
            row.Cells[2].AddParagraph(billing.PaymentMethod.PaymentMethodToString());
            SetStyleBaseForBillingInformation(row.Cells[2]);

            // linha amount
            AddAmountForBilling(row.Cells[3], billing.Amount);

            if (string.IsNullOrWhiteSpace(billing.Notes) == false)
            {
                // adiciona linha
                var noteRow = table.AddRow();
                noteRow.Height = HEIGHT_ROW_BILLING_TABLE;

                noteRow.Cells[0].AddParagraph(billing.Notes);
                noteRow.Cells[0].Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.GRAY };
                noteRow.Cells[0].Shading.Color = ColorsHelper.GRAY_LIGHT;
                noteRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                noteRow.Cells[0].MergeRight = 2; // mesclar celulas
                noteRow.Cells[0].Format.LeftIndent = 10;

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }

        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly startDate, DateOnly endDate)
    {
        var document = new Document();

        document.Info.Title = string.Format(ResourceReportGenerationMessages.WEEKLY_BILLING_TITLE, startDate.ToString("dd/MM/yyyy"), endDate.ToString("dd/MM/yyyy"));
        document.Info.Author = "Rhayane";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.BEBASNEUE_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "ProfileBarberBoss.png");

        row.Cells[0].AddImage(pathFile);

        row.Cells[1].AddParagraph("Barbearia do Ricardo");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 25 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly startDate, DateOnly endDate, decimal totalBillings)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format(ResourceReportGenerationMessages.WEEKLY_BILLING, startDate.ToString("dd/MM/yyy"), endDate.ToString("dd/MM/yyy"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.ROBOTO_MEDIUM, Size = 15 });

        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalBillings}", new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 50 });
    }

    private Table CreateBillingTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private void AddBillingServiceName(Cell cell, string billingServiceName)
    {
        cell.AddParagraph(billingServiceName);
        cell.Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2; // mesclar celulas
        cell.Format.LeftIndent = 10;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.GREEN_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForBillingInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GRAY_MEDIUM;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddAmountForBilling(Cell cell, decimal amount)
    {
        cell.AddParagraph($"{CURRENCY_SYMBOL} {amount}");
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
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
