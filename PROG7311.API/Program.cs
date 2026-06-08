using Microsoft.EntityFrameworkCore;
using PROG7311.API.Data;
using PROG7311.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddHealthChecks();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddScoped<PROG7311.API.Services.AuthService>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<PROG7311.API.Repositories.IServiceRequestRepository, PROG7311.API.Repositories.ServiceRequestRepository>();

// Register services
builder.Services.AddHttpClient<PROG7311.API.Services.CurrencyService>();
builder.Services.AddScoped<PROG7311.API.Services.CurrencyService>();
builder.Services.AddScoped<PROG7311.API.Services.ServiceRequestService>();
builder.Services.AddScoped<PROG7311.API.Patterns.Strategies.ServiceRequestValidator>();
builder.Services.AddScoped<PROG7311.API.Patterns.Observers.ContractNotifier>();
builder.Services.AddScoped<PROG7311.API.Patterns.Observers.ManagerNotifier>();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

// Ensure database migrations are applied on startup (useful for Docker deployments)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PROG7311.API.Data.ApplicationDbContext>();
    var retries = 10;
    while (retries > 0)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($"DB not ready, retries left: {retries}. Error: {ex.Message}");
            await Task.Delay(3000);
        }
    }
}

app.Run();

// Expose the implicit Program class to integration tests
namespace PROG7311.API
{
    public partial class Program { }
}
