using BarberBoss.Application.UseCases.Billings.Reports.Pdf.Fonts;
using FluentAssertions;

namespace UseCases.Test.Billings.Reports.Pdf.Fonts;

public class BillingsReportFontResolverTest
{
    [Fact]
    public void Success_Default_Font_When_Font_Not_Found()
    {
        var resolver = new BillingsReportFontResolver();

        var result = resolver.GetFont("FonteInexistente");

        result.Should().NotBeNullOrEmpty();
    }
}
