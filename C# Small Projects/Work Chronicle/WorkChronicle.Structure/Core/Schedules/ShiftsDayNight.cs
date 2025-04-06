namespace WorkChronicle.Structure.Core.Schedules
{
    public class ShiftsDayNight: IEngineStrategy
    {
        private readonly IEngineHelper<ISchedule<IShift>> helper;
        private ShiftType firstShiftType;


        public ShiftsDayNight(IEngineHelper<ISchedule<IShift>> helper)
        {
            this.helper = helper;
        }

        public Task ApplySchedule(ISchedule<IShift> schedule, ScheduleConfiguration sc, bool isCurrentMonth)
        {
            sc.FirstShift.TryParseShiftType(out firstShiftType);

            IShift? dayShift = null;
            IShift? nightShift = null;
            DateTime dateTime = default;


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

                nightShift = new NightShift(ShiftType.NightShift,
                                            sc.StartDate.Year,
                                            sc.StartDate.Month,
                                            sc.StartDate.Day + 1,
                                            sc.ShiftConfiguration.StartNightShift,
                                            sc.ShiftConfiguration.TotalShiftHours,
                                            isCurrentMonth);
                schedule.AddShift(nightShift!);



            }
            else if (firstShiftType == ShiftType.NightShift)
            {
                nightShift = new NightShift(ShiftType.NightShift, 
                                            sc.StartDate.Year,
                                            sc.StartDate.Month, 
                                            sc.StartDate.Day, 
                                            sc.ShiftConfiguration.StartNightShift, 
                                            sc.ShiftConfiguration.TotalShiftHours, 
                                            isCurrentMonth);

                schedule.AddShift(nightShift!);

                dateTime = new DateTime(sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day);
                dateTime = dateTime.Subtract(new TimeSpan(1, 0, 0, 0));
                dayShift = new DayShift(ShiftType.DayShift,
                                        dateTime.Year,
                                        dateTime.Month,
                                        dateTime.Day,
                                        sc.ShiftConfiguration.StartDayShift,
                                        sc.ShiftConfiguration.TotalShiftHours,
                                        isCurrentMonth);
            }

            //if (dayShift == null)
           // {
                //dayShift = new DayShift(ShiftType.DayShift, 
                //                        sc.StartDate.Year, 
                //                        sc.StartDate.Month, 
                //                        sc.StartDate.Day - 1, 
                //                        sc.ShiftConfiguration.StartDayShift, 
                //                        sc.ShiftConfiguration.TotalShiftHours, 
                //                        isCurrentMonth);
           // }
            //else if (nightShift == null)
            //{
                //nightShift = new NightShift(ShiftType.NightShift, 
                //                            sc.StartDate.Year, 
                //                            sc.StartDate.Month, 
                //                            sc.StartDate.Day + 1, 
                //                            sc.ShiftConfiguration.StartNightShift, 
                //                            sc.ShiftConfiguration.TotalShiftHours, 
                //                            isCurrentMonth);
                //schedule.AddShift(nightShift!);
          //  }

             this.helper
                .AddShiftsToSchedule(schedule,sc, dayShift, nightShift!, RestDaysBetweenShifts.DayNight);

            return Task.CompletedTask;
        }
    }
}
