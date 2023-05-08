using AvaiabilityReportApi.Data;
using AvaiabilityReportApi.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using Hangfire;
using Hangfire.SqlServer;

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

builder.Services.AddHangfire(configuration => configuration
       .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
       .UseSimpleAssemblyNameTypeSerializer()
       .UseRecommendedSerializerSettings()
       .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
       {
           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
           QueuePollInterval = TimeSpan.Zero,
           UseRecommendedIsolationLevel = true,
           DisableGlobalLocks = true
       }));
builder.Services.AddScoped<IAvaiabilityReportRepository, AvaiabilityReportRepository>();
builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.UseAuthorization();
RecurringJob.AddOrUpdate<IAvaiabilityReportRepository>("Load-AvaiabilityReportFactSt Table" , x => x.LoadAvaiabilityFactSt(), "30 23 * * *");
BackgroundJob.Enqueue<IAvaiabilityReportRepository>(x => x.LoadAvaiabilityFactSt());
app.MapControllers();

app.Run();
