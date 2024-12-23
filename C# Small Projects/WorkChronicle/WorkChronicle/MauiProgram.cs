using Microsoft.Extensions.Logging;
using WorkChronicle.Logic.Core;
using WorkChronicle.Logic.Core.Contacts;

namespace WorkChronicle
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
           builder.Logging.AddDebug();
            builder.Services.AddSingleton<IEngine, Engine>();
#endif

            return builder.Build();
        }
    }
}
