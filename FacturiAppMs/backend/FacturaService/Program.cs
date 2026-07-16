using FacturaService.Data;
using FacturaService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FacturaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpClient tipat catre ClientService (URL din appsettings.json -> "Services:ClientService")
builder.Services.AddHttpClient<ClientApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:ClientService"]!);
});

builder.Services.AddCors(options =>
    options.AddPolicy("Deschis", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Deschis");

app.UseAuthorization();
app.MapControllers();

app.Run();
