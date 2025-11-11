using BZDesktopApp;
using BZDesktopApp.Api.Services;
using BZDesktopApp.Api.Services.Interfaces;
using BZDesktopApp.Facades;
using BZDesktopApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("Poppins-Regular.ttf", "Poppins");
                fonts.AddFont("Poppins-Medium.ttf", "PoppinsMedium");
                fonts.AddFont("Poppins-SemiBold.ttf", "PoppinsSemiBold");
                fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // FORCE LOCAL OR FORCE LOCAL IF DEVELOPMENT
        string localConn = "Server=localhost;Database=bz_desktop;Trusted_Connection=True;TrustServerCertificate=True;";
        builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(localConn);
        });


        //// --- CLOUD + LOCAL ---
        //string localConn = "Server=localhost;Database=bz_desktop;Trusted_Connection=True;TrustServerCertificate=True;";
        //string cloudConn = "Server=db32081.public.databaseasp.net;Database=db32081;User Id=db32081;Password=Yr4_=Fh65?Am;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

        //// --- Register a DbContext factory ---
        //builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        //{
        //    bool hasInternet = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        //    string selectedConn = hasInternet ? cloudConn : localConn;
        //    options.UseSqlServer(selectedConn);
        //});


        // --- Register Services & Facades ---
        // Register your service implementing IAuthService
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        // Register your facade
        builder.Services.AddSingleton<AuthFacade>();

        // Register other services/facades
        builder.Services.AddSingleton<ICategoryService, CategoryService>();
        builder.Services.AddSingleton<IProductService, ProductService>();
        builder.Services.AddScoped<AuthenticationService>();
        builder.Services.AddSingleton<CategoriesFacade>();
        builder.Services.AddSingleton<ProductsFacade>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            BZDesktopApp.Api.Seed.DbSeeder.Seed(dbContext);
        }

        return app;
    }
}
