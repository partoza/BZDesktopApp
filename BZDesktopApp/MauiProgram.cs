using Microsoft.Extensions.Logging;

namespace BZDesktopApp
{
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
            builder.Services.AddMauiBlazorWebView();

            // register HttpClient for Blazor code (adjust BaseAddress to your API's address)
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7028/") });

            // auth service must be scoped (so IJSRuntime/HttpClient can be injected safely)
            builder.Services.AddScoped<BZDesktopApp.Services.AuthenticationService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
