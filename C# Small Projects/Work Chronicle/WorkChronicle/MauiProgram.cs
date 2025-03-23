namespace WorkChronicle
{
    using WorkChronicle.Data.Connection;
    using WorkChronicle.Data.RepositoryDB;
    using WorkChronicle.Structure.Models.Contracts;
    using WorkChronicle.Structure.Repository;
    using WorkChronicle.Structure.Repository.Contracts;

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
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<ISchedule<IShift>, Schedule>();
            builder.Services.AddSingleton<ScheduleDbContext>();
            builder.Services.AddSingleton<ScheduleRepositoryDB>();
            RegisterVM(builder.Services);
            RegisterPages(builder.Services);

            return builder.Build();
        }
        static void RegisterPages(IServiceCollection services)
        {
            services.AddTransient<MainPage>();
            services.AddTransient<SchedulePage>();
            services.AddTransient<PickerDatePage>();
            services.AddTransient<LoadSavedSchedulePage>();
            services.AddTransient<CompensateShiftsPage>();
        }

        static void RegisterVM(IServiceCollection services)
        {
            services.AddTransient<MainPageViewModel>();
            services.AddTransient<SchedulePageViewModel>();
            services.AddTransient<PickerDatePageViewModel>();
            services.AddTransient<LoadSavedSchedulePageViewModel>();
            services.AddTransient<CompensateShiftsPageViewModel>();
        }
    }
}
