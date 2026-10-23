using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Billings.DeleteBilling;
using BarberBoss.Application.UseCases.Billings.GetAllBillings;
using BarberBoss.Application.UseCases.Billings.GetById;
using BarberBoss.Application.UseCases.Billings.RegisterBilling;
using BarberBoss.Application.UseCases.Billings.Reports.Excel;
using BarberBoss.Application.UseCases.Billings.Reports.Pdf;
using BarberBoss.Application.UseCases.Billings.UpdateBilling;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(ctg => { }, typeof(AutoMapping));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterBillingUseCase, RegisterBillingUseCase>();
        services.AddScoped<IGetAllBillingUseCase, GetAllBillingUseCase>();
        services.AddScoped<IGetByIdBillingUseCase, GetByIdBillingUseCase>();
        services.AddScoped<IUpdateBillingUseCase, UpdateBillingUseCase>();
        services.AddScoped<IDeleteBillingUseCase, DeleteBillingUseCase>();
        services.AddScoped<IBillingsReportExcelUseCase, BillingsReportExcelUseCase>();
        services.AddScoped<IBillingsReportPdfUseCase, BillingsReportPdfUseCase>();
    }
}
