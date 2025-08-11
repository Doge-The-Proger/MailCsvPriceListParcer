using MailCsvPriceListParcer;
using MailCsvPriceListParcer.Interfaces;
using MailCsvPriceListParcer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqLiteConnection")));

builder.Services.AddSingleton<IMailProcessService,  MailProcessService>();
builder.Services.AddScoped<ICSVParcerService, CSVParcerService>();
builder.Services.AddScoped<IDatabaseManagerService, DatabaseManagerService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
