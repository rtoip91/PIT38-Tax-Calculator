using ExcelReader.Factory;
using ExcelReader.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelReader.Statics
{
    public static class ExcelReaderServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IExcelDataExtractor, ExcelDataExtractor>();
            services.AddTransient<IExcelFileHandler, ExcelFileHandler>();
            services.AddTransient<IExtractedDataProcessor, ExtractedDataProcessor>();
            services.AddTransient<IConverterFactory, ConverterFactory>();
        }
    }
}
