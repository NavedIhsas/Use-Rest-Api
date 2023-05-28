using Catologs.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()

    .CreateLogger();

Log.Information("شروع راه اندازی");
try
{

    var builder = WebApplication.CreateBuilder(args);
 
    builder.Host.UseSerilog().ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Trace);
    });
    var configuration = builder.Configuration;
    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
    

    builder.Services.AddRazorPages();
    builder.Services.AddHttpClient();
    builder.Services.AddHttpContextAccessor();


    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.Lax;
    });

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
        {
            o.LoginPath = new PathString("/Login");
            o.LogoutPath = new PathString("/Login");
            o.AccessDeniedPath = new PathString("/Error");
            o.ExpireTimeSpan = TimeSpan.FromDays(1);
        });



    var baseUrl = configuration["BaseUrl:url"];
    builder.Services.AddScoped<ICatalogs>(p=>new Catalog(new RestClient(baseUrl),configuration));
    builder.Services.AddHttpClient();
    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseStatusCodePages();
    app.UseAuthorization();

    app.UseCookiePolicy();
    app.UseAuthorization();
    app.MapRazorPages();

    app.Run();

}

catch (Exception ex)
{
    Log.Fatal(ex, "اجرایی اپلیکشن با خطای زیر مواجه شد.");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
