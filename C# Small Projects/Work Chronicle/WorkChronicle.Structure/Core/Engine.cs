namespace WorkChronicle.Structure.Core
{
    public class Engine : IEngine<ISchedule<IShift>>
    {
        private readonly ShiftPattern shiftPattern = new ShiftPattern();
        private ISchedule<IShift> schedule = new Schedule();

        private readonly IEngineHelper<ISchedule<IShift>> helper;
        private readonly EngineStrategyFactory factory;

        public Engine()
        {
            this.helper = new EngineHelper();
            factory = new EngineStrategyFactory(this.helper);
        }
        public async Task<ISchedule<IShift>> CalculateShifts(ScheduleConfiguration scheduleConfiguration)
        {
            await shiftPattern
                        .ChechTheShiftPattern(scheduleConfiguration.Cycle);

            var strategySchedule = await factory
                                            .GetStrategy(shiftPattern);

            await strategySchedule
                        .ApplySchedule(this.schedule, scheduleConfiguration, isCurrentMonth: true);

            this.schedule= await this.helper
                                            .GenerateMonthSchedule(this.schedule, scheduleConfiguration);

            return this.schedule;
        }
    }
}
