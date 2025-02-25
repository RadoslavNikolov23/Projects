
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


            builder.Services.AddSingleton<ISchedule<IShift>, Schedule>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ScheduleView>();
            builder.Services.AddSingleton<PickerDateView>();
            builder.Services.AddSingleton<LoadSavedScheduleView>();
            builder.Services.AddSingleton<CompensateShiftsView>();
        
#if DEBUG
           builder.Logging.AddDebug();
#endif


            return builder.Build();
        }
    }
}
