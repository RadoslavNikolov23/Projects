namespace WorkChronicle.Structure.Core
{
    public class Engine : IEngine<ISchedule<IShift>>
    {
        private ISchedule<IShift> schedule = new Schedule();

        public async Task<ISchedule<IShift>> CalculateShifts (ScheduleConfiguration scheduleConfiguration)
        {
            if (scheduleConfiguration.Cycle.Length == 2)
            {
                if (scheduleConfiguration.Cycle[1] == "Night")
                {
                    await ShiftsDayNight(scheduleConfiguration);
                }
                else if (scheduleConfiguration.Cycle[1] == "Day")
                {
                    await ShiftsDayDay(scheduleConfiguration);
                }
            }
            else if (scheduleConfiguration.Cycle.Length == 3)
            {
                await ShiftsDayNightNight(scheduleConfiguration);
            }

            return this.schedule;
        }

        //public int CalculateTotalHours(ISchedule<IShift> schedule) //TODO: Check if this method is necessary
        //{
        //    int totalHours = schedule.WorkSchedule.Sum(s => s.Hour);

        //    return totalHours;
        //}

        private async Task ShiftsDayNightNight(ScheduleConfiguration sc)
        {
            IShift? dayShift = null;
            IShift? firstNightShift = null;
            IShift? secondNightShift = null;

            if (sc.FirstShift == "DayShift")
            {
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(dayShift);
            }
            else if (sc.FirstShift == "NightShift")
            {
            //Chech if the cucle.length is 3 and then add the second night shift!
                firstNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
                secondNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(firstNightShift!);
                await this.schedule.AddShift(secondNightShift!); 
            }

            if (dayShift == null)
            {
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day - 1, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
            }
            else if (firstNightShift == null)
            {
                firstNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
                secondNightShift = new NightShift(ShiftType.NightShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 2, sc.ShiftConfiguration.StartNightShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(firstNightShift!);
                await this.schedule.AddShift(secondNightShift!);
            }

            const int daysBetweenShiftsCycle = 5;
            await AddShiftsToSchedule(sc.Cycle, dayShift, firstNightShift!, daysBetweenShiftsCycle, sc.StartDate.Month);
        }

        private async Task ShiftsDayDay(ScheduleConfiguration sc)
        {
            //Have to insert in the picker, aditionl option to pick where the dayShifts start - on the first/second?????)

            IShift firstDayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
            IShift secondDayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day + 1, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
            await this.schedule.AddShift(firstDayShift);
            await this.schedule.AddShift(secondDayShift);

            const int daysBetweenShiftsCycle = 4;
            await AddShiftsToSchedule(sc.Cycle, firstDayShift, secondDayShift!, daysBetweenShiftsCycle, sc.StartDate.Month);

        }

        private async Task ShiftsDayNight(ScheduleConfiguration sc)
        {
            IShift? dayShift = null;
            IShift? nightShift = null;

            if (sc.FirstShift == "DayShift")
            {
                dayShift = new DayShift(ShiftType.DayShift, sc.StartDate.Year, sc.StartDate.Month, sc.StartDate.Day, sc.ShiftConfiguration.StartDayShift, sc.ShiftConfiguration.TotalShiftHours);
                await this.schedule.AddShift(dayShift);
            }
            else if (sc.FirstShift == "NightShift")
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

            const int daysBetweenShiftsCycle = 4;
            await AddShiftsToSchedule(sc.Cycle, dayShift, nightShift!, daysBetweenShiftsCycle, sc.StartDate.Month);
        }

        private Task AddShiftsToSchedule(string[] cycle, IShift dayShift, IShift nightShift, int daysBetweenShiftsCycle, int startDateMonth)
        {
            bool IsDayDayShift = cycle.All(c => c == "DayShift");
            bool IsDayNightNightShift = cycle.Length == 3;

            DateTime tempDayDateTime = new DateTime(dayShift.Year, dayShift.Month, dayShift.Day);
            DateTime tempNightDateTime = default;
            DateTime tempSecondNightDateTime = default;

            if (!IsDayNightNightShift)
            {
                tempNightDateTime = new DateTime(nightShift.Year, nightShift.Month, nightShift.Day);
            }
            else
            {
                tempNightDateTime = new DateTime(nightShift.Year, nightShift.Month, nightShift.Day);
                tempSecondNightDateTime = new DateTime(nightShift.Year, nightShift.Month, nightShift.Day + 1);
            }

            int counter = 0;

            while (true)
            {
                string shift = cycle[counter++ % cycle.Length];

                if (shift == "Day")
                {
                    tempDayDateTime = tempDayDateTime.AddDays(daysBetweenShiftsCycle);

                    if (HasShiftMonthChanged(tempDayDateTime.Month, startDateMonth))
                        break;

                    this.schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDateTime.Year, tempDayDateTime.Month, tempDayDateTime.Day,dayShift.StarTime,dayShift.ShiftHour));

                    if (IsDayDayShift)
                    {
                        tempDayDateTime = tempDayDateTime.AddDays(1);

                        if (HasShiftMonthChanged(tempDayDateTime.Month, startDateMonth))
                            break;

                        this.schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDateTime.Year, tempDayDateTime.Month, tempDayDateTime.Day, dayShift.StarTime, dayShift.ShiftHour));
                    }
                }

                if (shift == "Night")
                {
                    tempNightDateTime = tempNightDateTime.AddDays(daysBetweenShiftsCycle);

                    if (HasShiftMonthChanged(tempNightDateTime.Month, startDateMonth))
                        break;

                    this.schedule.AddShift(new NightShift(nightShift.ShiftType, tempNightDateTime.Year, tempNightDateTime.Month, tempNightDateTime.Day, nightShift.StarTime, nightShift.ShiftHour));

                    if (IsDayNightNightShift)
                    {
                        tempSecondNightDateTime = tempSecondNightDateTime.AddDays(daysBetweenShiftsCycle);

                        if (HasShiftMonthChanged(tempSecondNightDateTime.Month, startDateMonth))
                            break;

                        this.schedule.AddShift(new NightShift(nightShift.ShiftType, tempSecondNightDateTime.Year, tempSecondNightDateTime.Month, tempSecondNightDateTime.Day, nightShift.StarTime, nightShift.ShiftHour));
                    }
                }
            }

            return Task.CompletedTask;
        }

        private bool HasShiftMonthChanged(int shiftMonth, int startDateMonth)
        {
            return shiftMonth != startDateMonth;
        }

    }
}
