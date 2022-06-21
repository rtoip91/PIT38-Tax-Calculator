using Calculations.Statics;
using Database;
using TaxEtoro.Interfaces;
using TaxEtoro.Statics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Logging.AddDebug();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

TaxEtoroServiceRegistration.RegisterServices(builder.Services);
CalculationsServicesRegistration.RegisterServices(builder.Services);
DatabaseServiceRegistration.RegisterServices(builder.Services);

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

var actionPerformer = app.Services.GetService<IActionPerformer>();
actionPerformer.PerformCalculationsAndWriteResultsPeriodically(app.Services);

app.Run();