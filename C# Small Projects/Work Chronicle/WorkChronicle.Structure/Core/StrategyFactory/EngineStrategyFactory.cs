namespace WorkChronicle.Structure.Core.StrategyFactory
{
    public class EngineStrategyFactory
    {
        private readonly IEngineHelper<ISchedule<IShift>> helper;

        public EngineStrategyFactory(IEngineHelper<ISchedule<IShift>> helper)
        {
            this.helper = helper;
        }

        public async Task<IEngineStrategy> GetStrategy(ShiftPattern pattern)
        {
            await Task.Delay(50); // Simulate some async work

            if (pattern.Is24Dayshift) 
                return new ShiftsDay24(this.helper);
            else if (pattern.IsDayDayShift) 
                return new ShiftsDayDay(this.helper);
            else if (pattern.IsDayNightShift) 
                return new ShiftsDayNight(this.helper);
            else 
                return new ShiftsDayNightNight(this.helper);
        }
    }
}
