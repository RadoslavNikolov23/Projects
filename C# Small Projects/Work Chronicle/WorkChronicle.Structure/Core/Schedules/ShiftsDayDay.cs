namespace WorkChronicle.Structure.Core.Schedules
{
    public class ShiftsDayDay : IEngineStrategy
    {
        private readonly IEngineHelper<ISchedule<IShift>> helper;

        public ShiftsDayDay(IEngineHelper<ISchedule<IShift>> helper)
        {
            this.helper = helper;
        }
        public Task ApplySchedule(ISchedule<IShift> schedule, ScheduleConfiguration sc, bool isCurrentMonth)
        {
            //Have to insert in the picker, aditionl option to pick where the dayShifts start - on the first/second?????)

            IShift firstDayShift = new DayShift(ShiftType.DayShift, 
                                                sc.StartDate.Year, 
                                                sc.StartDate.Month, 
                                                sc.StartDate.Day, 
                                                sc.ShiftConfiguration.StartDayShift, 
                                                sc.ShiftConfiguration.TotalShiftHours, 
                                                isCurrentMonth);

            IShift secondDayShift = new DayShift(ShiftType.DayShift,
                                                sc.StartDate.Year,
                                                sc.StartDate.Month,
                                                sc.StartDate.Day + 1,
                                                sc.ShiftConfiguration.StartDayShift,
                                                sc.ShiftConfiguration.TotalShiftHours,
                                                isCurrentMonth);

            schedule.AddShift(firstDayShift);
            schedule.AddShift(secondDayShift);

            this.helper.
                AddShiftsToSchedule(schedule, sc, firstDayShift, default, RestDaysBetweenShifts.DayDay);

            return Task.CompletedTask;
        }
    }
}
