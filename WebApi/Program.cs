using Calculations;
using Calculations.Interfaces;
using Database.DataAccess;
using Database.DataAccess.Interfaces;
using ExcelReader;
using ExcelReader.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IExcelDataExtractor, ExcelDataExtractor>();
builder.Services.AddTransient<IDataCleaner, DataCleaner>();

builder.Services.AddTransient<ITaxCalculations, TaxCalculations>();

builder.Services.AddTransient<IClosedPositionsDataAccess, ClosedPositionsDataAccess>();
builder.Services.AddTransient<ITransactionReportsDataAccess, TransactionReportsDataAccess>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
