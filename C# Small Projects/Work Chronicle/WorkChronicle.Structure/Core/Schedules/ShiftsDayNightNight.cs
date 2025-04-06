namespace WorkChronicle.Structure.Core.Schedules
{
    public class ShiftsDayNightNight:IEngineStrategy
    {
        private readonly IEngineHelper<ISchedule<IShift>> helper;
        private ShiftType firstShiftType;

        public ShiftsDayNightNight(IEngineHelper<ISchedule<IShift>> helper)
        {
            this.helper = helper;
        }
        public Task ApplySchedule( ISchedule<IShift> schedule, ScheduleConfiguration sc, bool isCurrentMonth)
        {
            sc.FirstShift.TryParseShiftType(out firstShiftType);

            IShift? dayShift = null;
            IShift? firstNightShift = null;
            IShift? secondNightShift = null;

            int daysInTheMonth = DateTime.DaysInMonth(sc.StartDate.Year, sc.StartDate.Month);


            if (firstShiftType == ShiftType.DayShift)
            {
                dayShift = new DayShift(ShiftType.DayShift, 
                                        sc.StartDate.Year, 
                                        sc.StartDate.Month, 
                                        sc.StartDate.Day, 
                                        sc.ShiftConfiguration.StartDayShift, 
                                        sc.ShiftConfiguration.TotalShiftHours,
                                        isCurrentMonth);
                schedule.AddShift(dayShift);

                //-----------To Check if it last day of the month!!!!------
                firstNightShift = new NightShift(ShiftType.NightShift, 
                                                sc.StartDate.Year, 
                                                sc.StartDate.Month, 
                                                sc.StartDate.Day + 1, 
                                                sc.ShiftConfiguration.StartNightShift, 
                                                sc.ShiftConfiguration.TotalShiftHours, 
                                                isCurrentMonth);

                schedule.AddShift(firstNightShift!);


                if (sc.StartDate.Day + 2 <= daysInTheMonth)
                {
                    secondNightShift = new NightShift(ShiftType.NightShift, 
                                                    sc.StartDate.Year, 
                                                    sc.StartDate.Month, 
                                                    sc.StartDate.Day + 2, 
                                                    sc.ShiftConfiguration.StartNightShift, 
                                                    sc.ShiftConfiguration.TotalShiftHours, 
                                                    isCurrentMonth);

                    schedule.AddShift(secondNightShift!);
                }

            }
            else
            {
                firstNightShift = new NightShift(ShiftType.NightShift, 
                                                sc.StartDate.Year, 
                                                sc.StartDate.Month, 
                                                sc.StartDate.Day, 
                                                sc.ShiftConfiguration.StartNightShift, 
                                                sc.ShiftConfiguration.TotalShiftHours, 
                                                isCurrentMonth);

                secondNightShift = new NightShift(ShiftType.NightShift, 
                                                sc.StartDate.Year, 
                                                sc.StartDate.Month, 
                                                sc.StartDate.Day + 1,
                                                sc.ShiftConfiguration.StartNightShift, 
                                                sc.ShiftConfiguration.TotalShiftHours, 
                                                isCurrentMonth);

                schedule.AddShift(firstNightShift!);
                schedule.AddShift(secondNightShift!);

                DateTime dateTime = new DateTime(sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day);
                dateTime = dateTime.Subtract(new TimeSpan(1, 0, 0, 0));

                dayShift = new DayShift(ShiftType.DayShift, 
                                        dateTime.Year,
                                        dateTime.Month, 
                                        dateTime.Day, 
                                        sc.ShiftConfiguration.StartDayShift, 
                                        sc.ShiftConfiguration.TotalShiftHours, 
                                        isCurrentMonth);


            }

            //int prevMonthDays = DateTime.DaysInMonth(sc.StartDate.Year, sc.StartDate.Month);

            //if (dayShift == null)
            //{
            //    DateTime dateTime = new DateTime(sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day);
            //    dateTime = dateTime.Subtract(new TimeSpan(1, 0, 0, 0));

            //    dayShift = new DayShift(ShiftType.DayShift, dateTime.Year, dateTime.Month, dateTime.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
            //}
            //else if (firstNightShift == null && sc.StartDate.Day + 1 <= prevMonthDays)
            //{
            //    firstNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);

            //    schedule.AddShift(firstNightShift!);


            //    if (sc.StartDate.Day + 2 <= prevMonthDays)
            //    {
            //        secondNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 2, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
            //        schedule.AddShift(secondNightShift!);
            //    }

            //}

            this.helper.
               AddShiftsToSchedule(schedule,sc, dayShift, firstNightShift!, RestDaysBetweenShifts.DayNightNight);

            return Task.CompletedTask;
        }
    }
}
