namespace WorkChronicle.Structure.Core.Helpers
{
    public class EngineHelper : IEngineHelper<ISchedule<IShift>>
    {
        private const bool isCurrentMonth = true;
        private const bool isNotCurrentMonth = false;

       // private ShiftType firstShiftType;
        private readonly ShiftPattern shiftPattern = new ShiftPattern();


        public Task<ISchedule<IShift>> AddShiftsToSchedule(ISchedule<IShift> schedule, ScheduleConfiguration sc, IShift dayShift, IShift? nightShift, int daysBetweenShiftsCycle)
        {
            string[] cycle=sc.Cycle;
            int startDateMonth = sc.StartDate.Month;

           // ShiftTypeExtensions.TryParseShiftType(sc.FirstShift, out firstShiftType);
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



            //TO DELETE !!!!!!
                        //if (firstShiftType == ShiftType.DayShift)
                        //{
                        //    tempDayDT = new DateTime(dayShift.Year, dayShift.Month, dayShift.Day);
                        //    tempNightDT = new DateTime(nightShift!.Year, nightShift.Month, nightShift.Day);
                        //}
                        //else
                        //{
                        //    tempNightDT = new DateTime(nightShift!.Year, nightShift.Month, nightShift.Day);
                        //}

                        //if (!shiftPattern.Is24Dayshift)
                        //{
                        //    if (firstShiftType==ShiftType.DayShift)
                        //    {
                        //        tempNightDT = new DateTime(nightShift!.Year, nightShift.Month, nightShift.Day);
                        //    }
                        //    else
                        //    {
                        //        //if (shiftPattern.IsDayNightNightShift)
                        //       // {
                        //       //     tempDayDT = new DateTime(dayShift.Year, dayShift.Month, dayShift.Day);

                        //      //  }

                        //        dateTime = dateTime.AddDays(1);

                        //        if (!HasShiftMonthChanged(dateTime.Month, nightShift!.Month))
                        //        {
                        //            tempSecondNightDT = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
                        //        }
                        //    }
                        //}

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

                    schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDT.Year, tempDayDT.Month, tempDayDT.Day, dayShift.StarTime, dayShift.ShiftHour, isCurrentMonth));

                    if (shiftPattern.IsDayDayShift)
                    {
                        tempDayDT = tempDayDT.AddDays(1);

                        if (HasShiftMonthChanged(tempDayDT.Month, startDateMonth))
                            break;

                        schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDT.Year, tempDayDT.Month, tempDayDT.Day, dayShift.StarTime, dayShift.ShiftHour, isCurrentMonth));
                    }
                }

                if (shift == "Night")
                {
                    tempNightDT = tempNightDT.AddDays(daysBetweenShiftsCycle);

                    if (HasShiftMonthChanged(tempNightDT.Month, startDateMonth))
                        break;

                    schedule.AddShift(new NightShift(nightShift!.ShiftType, tempNightDT.Year, tempNightDT.Month, tempNightDT.Day, nightShift.StarTime, nightShift.ShiftHour, isCurrentMonth));

                    if (shiftPattern.IsDayNightNightShift)
                    {
                        tempSecondNightDT = tempSecondNightDT.AddDays(daysBetweenShiftsCycle);

                        if (HasShiftMonthChanged(tempSecondNightDT.Month, startDateMonth))
                            break;

                        schedule.AddShift(new NightShift(nightShift.ShiftType, tempSecondNightDT.Year, tempSecondNightDT.Month, tempSecondNightDT.Day, nightShift.StarTime, nightShift.ShiftHour, isCurrentMonth));
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

            //--------Generate Previous Month Days, which are typed RestDays---------
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int offsetStart = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            var prevYearMonth = firstDayOfMonth.AddMonths(-1);
            int prevMonthDays = DateTime.DaysInMonth(prevYearMonth.Year, prevYearMonth.Month);

            for (int i = offsetStart - 1; i >= 0; i--)
            {
                IShift? shift = new RestDay(ShiftType.RestDay, prevYearMonth.Year, prevYearMonth.Month, prevMonthDays - i, 0, 0, isNotCurrentMonth);
                scheduleNew.AddShift(shift);
            }

            //--------Generate Current Month Days, which are the Work days and Rest days also ---------
            int totalDaysInMonth = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= totalDaysInMonth; i++)
            {
                IShift? shift = schedule.WorkSchedule.FirstOrDefault(w => w.Day == i);

                if (shift == null)
                {
                    shift = new RestDay(ShiftType.RestDay, year, month, i, 0, 0, isCurrentMonth);
                }

                scheduleNew.AddShift(shift);
            }

            //--------Generate Next Month Days, which are typed RestDays---------
            var nextYearMonth = firstDayOfMonth.AddMonths(1);
            int totalDays = scheduleNew.WorkSchedule.Count;
            int paddingEnd = 42 - totalDays;

            for (int i = 1; i <= paddingEnd; i++)
            {
                IShift? shift = new RestDay(ShiftType.RestDay, nextYearMonth.Year, nextYearMonth.Month, prevMonthDays - i, 0, 0, isNotCurrentMonth);
                scheduleNew.AddShift(shift);
            }

            schedule = scheduleNew;

            return Task.FromResult(schedule);
        }

        private bool HasShiftMonthChanged(int shiftMonth, int startDateMonth)
        {
            return shiftMonth != startDateMonth;
        }
    }

}
