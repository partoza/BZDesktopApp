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
            builder.Services.AddSingleton<BZDesktopApp.Services.AuthenticationService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
