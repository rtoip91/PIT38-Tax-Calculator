using Calculations.Statics;
using Database;
using ExcelReader.Statics;
using TaxCalculatingService.Statics;
using WebApi.Helpers;

namespace WebApi.Extensions;

internal static class ServiceExtensions
{
    public static void RegisterApplicationServices(this IServiceCollection serviceCollection)
    {
        TaxEtoroServiceRegistration.RegisterServices(serviceCollection);
        CalculationsServicesRegistration.RegisterServices(serviceCollection);
        DatabaseServiceRegistration.RegisterServices(serviceCollection);
        ExcelReaderServiceRegistration.RegisterServices(serviceCollection);
        serviceCollection.AddTransient<IFileUploadHelper, FileUploadHelper>();
    }
}