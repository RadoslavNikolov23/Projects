namespace WorkChronicle.Structure.Core.Schedules
{
    public class ShiftsDay24: IEngineStrategy
    {
        private readonly IEngineHelper<ISchedule<IShift>> helper;

        public ShiftsDay24(IEngineHelper<ISchedule<IShift>> helper)
        {
            this.helper = helper;
        }

        public Task ApplySchedule(ISchedule<IShift> schedule, ScheduleConfiguration sc, bool isCurrentMonth)
        { 
            IShift firstDayShift = new DayShift(ShiftType.DayShift, 
                                                sc.StartDate.Year, 
                                                sc.StartDate.Month, 
                                                sc.StartDate.Day, 
                                                sc.ShiftConfiguration.StartDayShift, 
                                                sc.ShiftConfiguration.TotalShiftHours, 
                                                isCurrentMonth);

             schedule.AddShift(firstDayShift);

            this.helper
                .AddShiftsToSchedule(schedule, sc, firstDayShift, default, RestDaysBetweenShifts.Day24hours);

            return Task.CompletedTask;
        }

    }

}
