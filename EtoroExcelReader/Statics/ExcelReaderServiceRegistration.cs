using ExcelReader.Dictionaries;
using ExcelReader.Dictionaries.Interfaces;
using ExcelReader.Dto;
using ExcelReader.Factory;
using ExcelReader.Interfaces;
using ExcelReader.Validators;
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
            services.AddTransient<IExcelStreamValidator, ExcelStreamValidator>();
            services.AddScoped<ICurrentVersionData, CurrentVersionData>();
            services.AddSingleton<IVersionData, VersionData>();
        }
    }
}
