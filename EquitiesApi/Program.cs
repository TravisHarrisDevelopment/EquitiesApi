using EquitiesApi.Services;


var builder = WebApplication.CreateBuilder(args);

//Inject ApiToken
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddEnvironmentVariables("EQUITIESAPI");


// Add services to the container.
builder.Services.AddHttpClient("Iex", httpClient =>
{
    httpClient.BaseAddress = new Uri("")
}
//-----------below registered client moving to factory---------------
//builder.Services.AddHttpClient<IReturnsService, ReturnsService>();
//builder.Services.AddScoped<IAlphaService, AlphaService>();
//-------------------------------------------------------------------
//builder.Services.AddScoped<IReturnsService, ReturnsService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
