using Microsoft.EntityFrameworkCore;
using PersonalFinance.Core.Parsers;
using PersonalFinance.Data.Context;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Api.Services;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Register repositories
builder.Services.AddScoped<IFamilyRepository, FamilyRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Register services
builder.Services.AddScoped<IFamilyService, FamilyService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Register parsers
builder.Services.AddScoped<BankParserFactory>();

// Add CORS for LAN access
builder.Services.AddCors(options =>
{
    options.AddPolicy("LanAccess", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("LanAccess");
app.UseAuthorization();
app.MapControllers();

app.Run();
