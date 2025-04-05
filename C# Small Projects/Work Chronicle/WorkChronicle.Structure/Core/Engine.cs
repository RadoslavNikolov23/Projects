namespace WorkChronicle.Structure.Core
{
    public class Engine : IEngine<ISchedule<IShift>>
    {
        private const bool isCurrentMonth = true;

        private const bool isNotCurrentMonth = false;

        private ShiftType firstShiftType;
        private readonly ShiftPattern shiftPattern = new ShiftPattern();
        private ISchedule<IShift> schedule = new Schedule();

        public async Task<ISchedule<IShift>> CalculateShifts(ScheduleConfiguration scheduleConfiguration)
        {
            await shiftPattern.ChechTheShiftPattern(scheduleConfiguration.Cycle);
            await ShiftTypeExtensions.TryParseShiftType(scheduleConfiguration.FirstShift, out firstShiftType);

            if (shiftPattern.Is24Dayshift)
            {
                await ShiftDay24(scheduleConfiguration);
            }
            else if (shiftPattern.IsDayDayShift)
            {
                await ShiftsDayDay(scheduleConfiguration);
            }
            else if (shiftPattern.IsDayNightShift)
            {
                await ShiftsDayNight(scheduleConfiguration);

            }
            else if (shiftPattern.IsDayNightNightShift)
            {
                await ShiftsDayNightNight(scheduleConfiguration);
            }

            await GenerateMonthSchedule(scheduleConfiguration);

            return this.schedule;
        }

        private async Task ShiftDay24(ScheduleConfiguration sc)
        {
            IShift firstDayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
            await this.schedule.AddShift(firstDayShift);

            await AddShiftsToSchedule(sc.Cycle, firstDayShift, default, RestDaysBetweenShifts.Day24hoursSchedule, sc.StartDate.Month);
        }

        private async Task ShiftsDayDay(ScheduleConfiguration sc)
        {
            //Have to insert in the picker, aditionl option to pick where the dayShifts start - on the first/second?????)

            IShift firstDayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
            IShift secondDayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);

            await this.schedule.AddShift(firstDayShift);
            await this.schedule.AddShift(secondDayShift);

            await AddShiftsToSchedule(sc.Cycle, firstDayShift, secondDayShift!, RestDaysBetweenShifts.DayDaySchedule, sc.StartDate.Month);

        }

        private async Task ShiftsDayNight(ScheduleConfiguration sc)
        {
            IShift? dayShift = null;
            IShift? nightShift = null;

            if (firstShiftType == ShiftType.DayShift)
            {
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
                await this.schedule.AddShift(dayShift);
            }
            else if (firstShiftType == ShiftType.NightShift)
            {
                nightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
                await this.schedule.AddShift(nightShift!);
            }

            if (dayShift == null)
            {
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day - 1, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
            }
            else if (nightShift == null)
            {
                nightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
                await this.schedule.AddShift(nightShift!);
            }

            await AddShiftsToSchedule(sc.Cycle, dayShift, nightShift!, RestDaysBetweenShifts.DayNightSchedule, sc.StartDate.Month);
        }

        private async Task ShiftsDayNightNight(ScheduleConfiguration sc)
        {
            IShift? dayShift = null;
            IShift? firstNightShift = null;
            IShift? secondNightShift = null;

            if (firstShiftType == ShiftType.DayShift)
            {
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
                await this.schedule.AddShift(dayShift);

            }
            else if (firstShiftType == ShiftType.NightShift)
            {
                firstNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
                secondNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
                await this.schedule.AddShift(firstNightShift!);
                await this.schedule.AddShift(secondNightShift!);
            }

            int prevMonthDays = DateTime.DaysInMonth(sc.StartDate.Year, sc.StartDate.Month);

            if (dayShift == null)
            {
                DateTime dateTime = new DateTime(sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day);
                dateTime = dateTime.Subtract(new TimeSpan(1, 0, 0, 0));
               
                dayShift = new DayShift(ShiftType.DayShift, dateTime.Year, dateTime.Month, dateTime.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
            }
            else if (firstNightShift == null && sc.StartDate.Day + 1 <= prevMonthDays)
            {
                firstNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);

                await this.schedule.AddShift(firstNightShift!);


                if (sc.StartDate.Day + 2 <= prevMonthDays)
                {
                    secondNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 2, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours, isCurrentMonth);
                    await this.schedule.AddShift(secondNightShift!);
                }

            }

            await AddShiftsToSchedule(sc.Cycle, dayShift, firstNightShift!, RestDaysBetweenShifts.DayNightNightSchedule, sc.StartDate.Month);
        }

        private Task AddShiftsToSchedule(string[] cycle, IShift dayShift, IShift? nightShift, int daysBetweenShiftsCycle, int startDateMonth)
        {

            DateTime tempDayDT = default;
            DateTime tempNightDT = default;
            DateTime tempSecondNightDT = default;

            DateTime dateTime = default;


            if (firstShiftType == ShiftType.DayShift)
            {
                tempDayDT = new DateTime(dayShift.Year, dayShift.Month, dayShift.Day);
            }
            else
            {
                tempNightDT = new DateTime(nightShift!.Year, nightShift.Month, nightShift.Day);
                dateTime = new DateTime(nightShift.Year, nightShift.Month, nightShift.Day);
            }

            if (!shiftPattern.Is24Dayshift)
            {
                if (!shiftPattern.IsDayNightNightShift)
                {
                    tempNightDT = new DateTime(nightShift!.Year, nightShift.Month, nightShift.Day);
                }
                else
                {
                    if (shiftPattern.IsDayNightNightShift)
                    {
                        tempDayDT = new DateTime(dayShift.Year, dayShift.Month, dayShift.Day);

                    }

                    dateTime = dateTime.AddDays(1);

                    if (!HasShiftMonthChanged(dateTime.Month, nightShift!.Month))
                    {
                        tempSecondNightDT = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
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

                    this.schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDT.Year, tempDayDT.Month, tempDayDT.Day, dayShift.StarTime, dayShift.ShiftHour, isCurrentMonth));

                    if (shiftPattern.IsDayDayShift)
                    {
                        tempDayDT = tempDayDT.AddDays(1);

                        if (HasShiftMonthChanged(tempDayDT.Month, startDateMonth))
                            break;

                        this.schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDT.Year, tempDayDT.Month, tempDayDT.Day, dayShift.StarTime, dayShift.ShiftHour, isCurrentMonth));
                    }
                }

                if (shift == "Night")
                {
                    tempNightDT = tempNightDT.AddDays(daysBetweenShiftsCycle);

                    if (HasShiftMonthChanged(tempNightDT.Month, startDateMonth))
                        break;

                    this.schedule.AddShift(new NightShift(nightShift!.ShiftType, tempNightDT.Year, tempNightDT.Month, tempNightDT.Day, nightShift.StarTime, nightShift.ShiftHour, isCurrentMonth));

                    if (shiftPattern.IsDayNightNightShift)
                    {
                        tempSecondNightDT = tempSecondNightDT.AddDays(daysBetweenShiftsCycle);

                        if (HasShiftMonthChanged(tempSecondNightDT.Month, startDateMonth))
                            break;

                        this.schedule.AddShift(new NightShift(nightShift.ShiftType, tempSecondNightDT.Year, tempSecondNightDT.Month, tempSecondNightDT.Day, nightShift.StarTime, nightShift.ShiftHour, isCurrentMonth));
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task GenerateMonthSchedule(ScheduleConfiguration scheduleConfiguration)
        {
            ISchedule<IShift> scheduleNew = new Schedule();
            int year = scheduleConfiguration.StartDate.Year;
            int month = scheduleConfiguration.StartDate.Month;

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
                IShift? shift = this.schedule.WorkSchedule.FirstOrDefault(w => w.Day == i);

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

            this.schedule = scheduleNew;

            return Task.CompletedTask;
        }


        private bool HasShiftMonthChanged(int shiftMonth, int startDateMonth)
        {
            return shiftMonth != startDateMonth;
        }

    }
}
