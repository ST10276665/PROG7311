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
    builder.Services.AddTransient<FileService>();
    builder.Services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });
    builder.Services.AddControllers();

    // API integration: session, http client, and context accessor
    builder.Services.AddSession();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient<PROG7311.Services.ApiService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]!);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new System.Net.Http.HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = System.Net.Http.HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });
    builder.Services.AddScoped<PROG7311.Filters.SessionAuthFilter>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseSession();
    app.Use(async (ctx, next) =>
    {
        // Simple session auth middleware: redirect to login if no token
        var path = ctx.Request.Path.Value;
        if (!path.StartsWith("/Auth", System.StringComparison.OrdinalIgnoreCase))
        {
            var token = ctx.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                ctx.Response.Redirect("/Auth/Login");
                return;
            }
        }
        try
        {
            await next();
        }
        catch (UnauthorizedAccessException)
        {
            ctx.Session.Remove("JwtToken");
            ctx.Response.Redirect("/Auth/Login");
        }
    });
    app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();  