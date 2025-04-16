namespace WorkChronicle.Structure.Core.Main
{
    public class Engine : IEngine<ISchedule<IShift>>
    {
        private readonly ShiftPattern shiftPattern = new ShiftPattern();

        private readonly IEngineHelper<ISchedule<IShift>> helper;

        private readonly EngineStrategyFactory factory;

        private ISchedule<IShift> schedule = new Schedule();

        public Engine()
        {
            helper = new EngineHelper();
            factory = new EngineStrategyFactory(helper);
        }

        public async Task<ISchedule<IShift>> CalculateShifts(ScheduleConfiguration scheduleConfiguration)
        {
            try
            {
                await shiftPattern.CheckTheShiftPattern(scheduleConfiguration.Cycle);

                var strategySchedule = await factory.GetStrategy(shiftPattern);

                await strategySchedule
                            .ApplySchedule(schedule, scheduleConfiguration, isCurrentMonth: true);

                schedule = await helper.GenerateMonthSchedule(schedule, scheduleConfiguration);

                return schedule;
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in CalculateShifts, in the Engine class.");
                throw;
            }
        }

        public async Task<ISchedule<IShift>> BlankCalendar()
        {
            try
            {
                schedule = await helper.GenerateBlankCalendar(schedule);

                return schedule;
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in BlankCalendar, in the Engine class");
                throw;
            }
        }
    }
}
