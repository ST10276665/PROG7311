using Microsoft.EntityFrameworkCore;
using PROG7311.Data;
using PROG7311.Patterns.Observers;
using PROG7311.Patterns.Strategies;
using PROG7311.Services;

var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new System.Globalization.CultureInfo("en-ZA");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddHttpClient<CurrencyService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<ServiceRequestValidator>();
builder.Services.AddScoped<ContractNotifier>();
builder.Services.AddScoped<ManagerNotifier>();
builder.Services.AddScoped<FileService>();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  