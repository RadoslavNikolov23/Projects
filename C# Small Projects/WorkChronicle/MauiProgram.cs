using Microsoft.Extensions.Logging;
using WorkChronicle.Core.Repository;
using WorkChronicle.Structure.Core;
using Microsoft.Extensions.DependencyInjection;
using WorkChronicle.Structure.Database;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Structure.Core.Contracts;


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
#endif
            builder.Services.AddSingleton<IEngine, Engine>();
            builder.Services.AddSingleton<Schedule>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ScheduleView>();
            builder.Services.AddSingleton<PickerDateView>();
            builder.Services.AddSingleton<CompensateShiftsView>();


            return builder.Build();
        }
    }
}
