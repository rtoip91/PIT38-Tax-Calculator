using Calculations.Statics;
using Database;
using ExcelReader.Statics;
using Serilog;
using TaxEtoro.Statics;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var logger = new LoggerConfiguration()
  .MinimumLevel.Override("Microsoft.AspNetCore",Serilog.Events.LogEventLevel.Warning)
  .MinimumLevel.Override("System.Net.Http.HttpClient", Serilog.Events.LogEventLevel.Warning)
  .MinimumLevel.Information()
  .WriteTo.Console()
  .WriteTo.File("../logs/log.txt", rollingInterval: RollingInterval.Day)
  .Enrich.FromLogContext()
  .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

TaxEtoroServiceRegistration.RegisterServices(builder.Services);
CalculationsServicesRegistration.RegisterServices(builder.Services);
DatabaseServiceRegistration.RegisterServices(builder.Services);
ExcelReaderServiceRegistration.RegisterServices(builder.Services);

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

app.RunPeriodicTasks();

app.Run();