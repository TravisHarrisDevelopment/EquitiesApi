using EquitiesApi.Services;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//Inject ApiToken
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables("EQUITIESAPI");

// Add services to the container.
builder.Services.AddHttpClient("Iex", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://cloud.iexapis.com/stable/time-series/HISTORICAL_PRICES/");
});

builder.Services.AddScoped<IReturnsService, ReturnsService>();
builder.Services.AddScoped<IAlphaService, AlphaService>();
//-----------below registered client moving to factory---------------
//builder.Services.AddHttpClient<IReturnsService, ReturnsService>();
//builder.Services.AddScoped<IAlphaService, AlphaService>();
//-------------------------------------------------------------------
//builder.Services.AddScoped<IReturnsService, ReturnsService>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Equities Api",
        Description = "An Api to with routes to retrieve stock returns for a given symbol and date range and " +
        "to retrive Alpha for a specified stock using another stock as benchmark for a given date range."
    });
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

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

