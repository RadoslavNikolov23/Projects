namespace WorkChronicle.Structure.Core
{
    public class Engine : IEngine<ISchedule<IShift>>
    {
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
            IShift firstDayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
            await this.schedule.AddShift(firstDayShift);

            await AddShiftsToSchedule(sc.Cycle, firstDayShift, default, RestDaysBetweenShifts.Day24hoursSchedule, sc.StartDate.Month);
        }

        private async Task ShiftsDayDay(ScheduleConfiguration sc)
        {
            //Have to insert in the picker, aditionl option to pick where the dayShifts start - on the first/second?????)

            IShift firstDayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
            IShift secondDayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);

            await this.schedule.AddShift(firstDayShift);
            await this.schedule.AddShift(secondDayShift);

            await AddShiftsToSchedule(sc.Cycle, firstDayShift, secondDayShift!, RestDaysBetweenShifts.DayDaychedule, sc.StartDate.Month);

        }

        private async Task ShiftsDayNight(ScheduleConfiguration sc)
        {
            IShift? dayShift = null;
            IShift? nightShift = null;

            if (firstShiftType == ShiftType.DayShift)
            {
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(dayShift);
            }
            else if (firstShiftType == ShiftType.NightShift)
            {
                nightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(nightShift!);
            }

            if (dayShift == null)
            {
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day - 1, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
            }
            else if (nightShift == null)
            {
                nightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
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
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(dayShift);

            }
            else if (firstShiftType == ShiftType.NightShift)
            {
                firstNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
                secondNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(firstNightShift!);
                await this.schedule.AddShift(secondNightShift!);
            }

            if (dayShift == null)
            {
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day - 1, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(dayShift);
            }
            else if (firstNightShift == null)
            {
                firstNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
                secondNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 2, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(firstNightShift!);
                await this.schedule.AddShift(secondNightShift!);
            }

            await AddShiftsToSchedule(sc.Cycle, dayShift, firstNightShift!, RestDaysBetweenShifts.DayNightNightSchedule, sc.StartDate.Month);
        }

        private Task AddShiftsToSchedule(string[] cycle, IShift dayShift, IShift? nightShift, int daysBetweenShiftsCycle, int startDateMonth)
        { 
            DateTime tempDayDT = new DateTime(dayShift.Year, dayShift.Month, dayShift.Day);
            DateTime tempNightDT = default;
            DateTime tempSecondNightDT = default;

            if (!shiftPattern.Is24Dayshift)
            {
                if (!shiftPattern.IsDayNightNightShift)
                {
                    tempNightDT = new DateTime(nightShift!.Year, nightShift.Month, nightShift.Day);
                }
                else
                {
                    tempNightDT = new DateTime(nightShift!.Year, nightShift.Month, nightShift.Day);
                    tempSecondNightDT = new DateTime(nightShift.Year, nightShift.Month, nightShift.Day + 1);
                }
            }

            int counter = 0;

            while (true)
            {
                string shift = cycle[counter++ % cycle.Length];

                if (shift == "Day" || shiftPattern.Is24Dayshift)
                {
                    tempDayDT = tempDayDT.AddDays(daysBetweenShiftsCycle);

                    if (HasShiftMonthChanged(tempDayDT.Month, startDateMonth))
                        break;

                    this.schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDT.Year, tempDayDT.Month, tempDayDT.Day, dayShift.StarTime, dayShift.ShiftHour));

                    if (shiftPattern.IsDayDayShift)
                    {
                        tempDayDT = tempDayDT.AddDays(1);

                        if (HasShiftMonthChanged(tempDayDT.Month, startDateMonth))
                            break;

                        this.schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDT.Year, tempDayDT.Month, tempDayDT.Day, dayShift.StarTime, dayShift.ShiftHour));
                    }
                }

                if (shift == "Night")
                {
                    tempNightDT = tempNightDT.AddDays(daysBetweenShiftsCycle);

                    if (HasShiftMonthChanged(tempNightDT.Month, startDateMonth))
                        break;

                    this.schedule.AddShift(new NightShift(nightShift!.ShiftType, tempNightDT.Year, tempNightDT.Month, tempNightDT.Day, nightShift.StarTime, nightShift.ShiftHour));

                    if (shiftPattern.IsDayNightNightShift)
                    {
                        tempSecondNightDT = tempSecondNightDT.AddDays(daysBetweenShiftsCycle);

                        if (HasShiftMonthChanged(tempSecondNightDT.Month, startDateMonth))
                            break;

                        this.schedule.AddShift(new NightShift(nightShift.ShiftType, tempSecondNightDT.Year, tempSecondNightDT.Month, tempSecondNightDT.Day, nightShift.StarTime, nightShift.ShiftHour));
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task GenerateMonthSchedule(ScheduleConfiguration scheduleConfiguration)
        {
            ISchedule<IShift> scheduleNew = new Schedule();
            var year = scheduleConfiguration.StartDate.Year;
            var month = scheduleConfiguration.StartDate.Month;

            int totalDaysInMotn = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= totalDaysInMotn; i++)
            {
                IShift? shift = this.schedule.WorkSchedule.FirstOrDefault(w => w.Day == i);

                if (shift == null)
                {
                    shift = new RestDay(ShiftType.RestDay, year, month, i, 0, 0);
                }

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
