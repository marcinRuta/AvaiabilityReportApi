using AvaiabilityReportApi.Data;
using AvaiabilityReportApi.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var conStrBuilder = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("MainDatabaseConnection")) ?? throw new InvalidOperationException("Connection string 'MainDatabaseConnection' not found."); ;
conStrBuilder.Password = builder.Configuration["MainDatabasePassword"];
var connectionString = conStrBuilder.ConnectionString;
builder.Services.AddDbContext<GymAvaiabilityDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAvaiabilityReportRepository, AvaiabilityReportRepository>();



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
