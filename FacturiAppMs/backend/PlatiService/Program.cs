using Microsoft.EntityFrameworkCore;
using PlatiService.Data;
using PlatiService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PlatiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpClient tipat catre FacturaService (URL din appsettings.json -> "Services:FacturaService")
builder.Services.AddHttpClient<FacturaApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:FacturaService"]!);
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
