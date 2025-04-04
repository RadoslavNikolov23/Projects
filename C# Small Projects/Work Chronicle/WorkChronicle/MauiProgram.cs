﻿namespace WorkChronicle
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<ISchedule<IShift>, Schedule>();
            RegisterDateBaseComponents(builder.Services);
            RegisterVM(builder.Services);
            RegisterPages(builder.Services);

            return builder.Build();
        }
        static void RegisterDateBaseComponents(IServiceCollection services)
        {
            services.AddSingleton<WorkScheduleDB>();
            services.AddSingleton<WorkScheduleRepositoryDB>();
            services.AddSingleton<WorkShiftRepositoryDB>();
        }
        static void RegisterPages(IServiceCollection services)
        {
            services.AddTransient<MainPage>();
            services.AddTransient<SchedulePage>();
            services.AddTransient<PickerDatePage>();
            services.AddTransient<PropertiePage>();
            services.AddTransient<CompensateShiftsPage>();
        }

        static void RegisterVM(IServiceCollection services)
        {
            services.AddTransient<MainPageViewModel>();
            services.AddTransient<SchedulePageViewModel>();
            services.AddTransient<PickerDatePageViewModel>();
            services.AddTransient<PropertieViewModel>();
            services.AddTransient<CompensateShiftsPageViewModel>();
        }
    }
}
