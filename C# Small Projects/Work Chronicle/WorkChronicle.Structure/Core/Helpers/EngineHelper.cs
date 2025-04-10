namespace WorkChronicle.Structure.Core.Helpers
{
    public class EngineHelper : IEngineHelper<ISchedule<IShift>>
    {
        private const bool isCurrentMonth = true;
        private const bool isNotCurrentMonth = false;

        private readonly ShiftPattern shiftPattern = new ShiftPattern();

        public Task<ISchedule<IShift>> AddShiftsToSchedule(ISchedule<IShift> schedule, ScheduleConfiguration sc, IShift dayShift, IShift? nightShift, int daysBetweenShiftsCycle)
        {
            string[] cycle=sc.Cycle;
            int startDateMonth = sc.StartDate.Month;

            shiftPattern.ChechTheShiftPattern(sc.Cycle);

            DateTime tempDayDT = new DateTime(dayShift.Year, dayShift.Month, dayShift.Day);
            DateTime tempNightDT = default;
            DateTime tempSecondNightDT = default;


            if (shiftPattern.IsDayNightShift || shiftPattern.IsDayNightNightShift)
            {
                tempNightDT = new DateTime(nightShift!.Year, nightShift.Month, nightShift.Day);

                if (shiftPattern.IsDayNightNightShift)
                {
                    DateTime newSecondNight = new DateTime(nightShift.Year, nightShift.Month, nightShift.Day+1);
                    if (!HasShiftMonthChanged(newSecondNight.Month, nightShift!.Month))
                    {
                        tempSecondNightDT = new DateTime(newSecondNight.Year, newSecondNight.Month, newSecondNight.Day);
                    }
                }
            }

            int counter = 0;

            if (shiftPattern.IsDayNightNightShift)
                cycle = new string[] { "Day", "Night" };

            while (true)
            {
                string shift = cycle[counter++ % cycle.Length];

                if (shift == "Day" || shiftPattern.Is24Dayshift)
                {
                    tempDayDT = tempDayDT.AddDays(daysBetweenShiftsCycle);

                    if (HasShiftMonthChanged(tempDayDT.Month, startDateMonth))
                        break;

                    schedule.AddShift(new DayShift(dayShift.ShiftType, 
                                                    tempDayDT.Year, 
                                                    tempDayDT.Month, 
                                                    tempDayDT.Day, 
                                                    dayShift.StarTime, 
                                                    dayShift.ShiftHour, 
                                                    isCurrentMonth));

                    if (shiftPattern.IsDayDayShift)
                    {
                        tempDayDT = tempDayDT.AddDays(1);

                        if (HasShiftMonthChanged(tempDayDT.Month, startDateMonth))
                            break;

                        schedule.AddShift(new DayShift(dayShift.ShiftType, 
                                                        tempDayDT.Year, 
                                                        tempDayDT.Month, 
                                                        tempDayDT.Day, 
                                                        dayShift.StarTime, 
                                                        dayShift.ShiftHour, 
                                                        isCurrentMonth));
                       
                        tempDayDT = tempDayDT.AddDays(-1);
                    }
                }

                if (shift == "Night")
                {
                    tempNightDT = tempNightDT.AddDays(daysBetweenShiftsCycle);

                    if (HasShiftMonthChanged(tempNightDT.Month, startDateMonth))
                        break;

                    schedule.AddShift(new NightShift(nightShift!.ShiftType, 
                                                     tempNightDT.Year, 
                                                     tempNightDT.Month, 
                                                     tempNightDT.Day, 
                                                     nightShift.StarTime, 
                                                     nightShift.ShiftHour, 
                                                     isCurrentMonth));

                    if (shiftPattern.IsDayNightNightShift)
                    {
                        tempSecondNightDT = tempSecondNightDT.AddDays(daysBetweenShiftsCycle);

                        if (HasShiftMonthChanged(tempSecondNightDT.Month, startDateMonth))
                            break;

                        schedule.AddShift(new NightShift(nightShift.ShiftType, 
                                                        tempSecondNightDT.Year, 
                                                        tempSecondNightDT.Month,
                                                        tempSecondNightDT.Day, 
                                                        nightShift.StarTime, 
                                                        nightShift.ShiftHour, 
                                                        isCurrentMonth));
                    }
                }
            }

            return Task.FromResult(schedule);
        }

        public Task<ISchedule<IShift>> GenerateMonthSchedule(ISchedule<IShift> schedule, ScheduleConfiguration sc)
        {
            ISchedule<IShift> scheduleNew = new Schedule();
            int year = sc.StartDate.Year;
            int month = sc.StartDate.Month;

            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            //--------Generate Previous Month Days, which are typed RestDays---------
            scheduleNew = GeneratePreviousMonthDays(scheduleNew, firstDayOfMonth);


            //--------Generate Current Month Days, which are the Work days and Rest days also ---------
            int totalDaysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= totalDaysInMonth; day++)
            {
                IShift? shift = schedule.WorkSchedule.FirstOrDefault(w => w.Day == day);

                if (shift == null)
                {
                    shift = new RestDay(ShiftType.RestDay, 
                                        year, 
                                        month, 
                                        day, 
                                        0, //StartTime
                                        0, //ShiftHour
                                        isCurrentMonth);
                }

                scheduleNew.AddShift(shift);
            }

            //--------Generate Next Month Days, which are typed RestDays---------
            scheduleNew = GenerateNextMonthDays(scheduleNew, firstDayOfMonth);

            schedule = scheduleNew;

            return Task.FromResult(schedule);
        }

      

        public Task<ISchedule<IShift>> GenerateBlankCalendar (ISchedule<IShift> schedule)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            //--------Generate Previous Month Days, which are typed RestDays---------
            schedule = (ISchedule<IShift>)GeneratePreviousMonthDays(schedule, firstDayOfMonth);

            //--------Generate Current Month Days, which are all Rest days ---------
            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                IShift shift = new RestDay(ShiftType.RestDay,
                                            year,
                                            month,
                                            day,
                                            0.0, //StartTime
                                            0.0,  //ShiftHour
                                            isCurrentMonth);

                schedule.WorkSchedule.Add(shift);
            }

            //--------Generate Next Month Days, which are typed RestDays---------
            schedule = (ISchedule<IShift>)GenerateNextMonthDays(schedule,firstDayOfMonth);

            return Task.FromResult(schedule);
        }

        private ISchedule<IShift> GeneratePreviousMonthDays(ISchedule<IShift> schedule, DateTime firstDayOfMonth)
        {
            int offsetStart = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            var prevMonth = firstDayOfMonth.AddMonths(-1);
            int prevMonthDays = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);

            for (int i = offsetStart - 1; i >= 0; i--)
            {
                IShift shift = new RestDay(ShiftType.RestDay,
                                            prevMonth.Year,
                                            prevMonth.Month,
                                            prevMonthDays - i,
                                            0.0, //StartTime
                                            0.0,  //ShiftHour
                                           isNotCurrentMonth);

                schedule.WorkSchedule.Add(shift);
            }

            return schedule;
        }
        private ISchedule<IShift> GenerateNextMonthDays(ISchedule<IShift> schedule, DateTime firstDayOfMonth)
        {
            int totalDays = schedule.WorkSchedule.Count();
            var nextMonth = firstDayOfMonth.AddMonths(1);
            int paddingEnd = 42 - totalDays;

            for (int day = 1; day <= paddingEnd; day++)
            {
                IShift shift = new RestDay(ShiftType.RestDay,
                                            nextMonth.Year,
                                            nextMonth.Month,
                                            day,
                                            0.0, //StartTime
                                            0.0, //ShiftHour
                                           isNotCurrentMonth);

                schedule.WorkSchedule.Add(shift);
            }

            return schedule;
        }

        private bool HasShiftMonthChanged(int shiftMonth, int startDateMonth)
        {
            return shiftMonth != startDateMonth;
        }
    }

}
