namespace WorkChronicle.Structure.Core
{
    public class Engine : IEngine<ISchedule<IShift>>
    {
        private ISchedule<IShift> schedule = new Schedule();

        public async Task<ISchedule<IShift>> CalculateShifts (DateTime startDate, string[] cycle, string firstShift)
        {
            if (cycle.Length == 2)
            {
                if (cycle[1] == "Night")
                {
                    await ShiftsDayNight(startDate, cycle, firstShift);
                }
                else if (cycle[1] == "Day")
                {
                    await ShiftsDayDay(startDate, cycle, firstShift);
                }
            }
            else if (cycle.Length == 3)
            {
                await ShiftsDayNightNight(startDate, cycle, firstShift);
            }

            return this.schedule;
        }

        public int CalculateTotalHours(ISchedule<IShift> schedule) //TODO: Check if this method is necessary
        {
            int totalHours = schedule.WorkSchedule.Sum(s => s.Hour);

            return totalHours;
        }

        private async Task ShiftsDayNightNight(DateTime startDate, string[] cycle, string firstShift)
        {
            IShift? dayShift = null;
            IShift? firstNightShift = null;
            IShift? secondNightShift = null;

            if (firstShift == "DayShift")
            {
                dayShift = new DayShift("DayShift", startDate.Year, startDate.Month, startDate.Day);
                await this.schedule.AddShift(dayShift);
            }
            else if (firstShift == "NightShift")
            {
                firstNightShift = new NightShift("NightShift", startDate.Year, startDate.Month, startDate.Day);
                secondNightShift = new NightShift("NightShift", startDate.Year, startDate.Month, startDate.Day + 1);
                await this.schedule.AddShift(firstNightShift!);
                await this.schedule.AddShift(secondNightShift!);
            }

            if (dayShift == null)
            {
                dayShift = new DayShift("DayShift", startDate.Year, startDate.Month, startDate.Day - 1);
            }
            else if (firstNightShift == null)
            {
                firstNightShift = new NightShift("NightShift", startDate.Year, startDate.Month, startDate.Day + 1);
                secondNightShift = new NightShift("NightShift", startDate.Year, startDate.Month, startDate.Day + 2);
                await this.schedule.AddShift(firstNightShift!);
                await this.schedule.AddShift(secondNightShift!);
            }

            const int daysBetweenShiftsCycle = 5;
            await AddShiftsToSchedule(cycle, dayShift, firstNightShift!, daysBetweenShiftsCycle, startDate.Month);
        }

        private async Task ShiftsDayDay(DateTime startDate, string[] cycle, string firstShift)
        {
            //Have to insert in the picker, aditionl option to pick where the dayShifts start - on the first/second?????)

            IShift firstDayShift = new DayShift("DayShift", startDate.Year, startDate.Month, startDate.Day);
            IShift secondDayShift = new DayShift("DayShift", startDate.Year, startDate.Month, startDate.Day + 1);
            await this.schedule.AddShift(firstDayShift);
            await this.schedule.AddShift(secondDayShift);

            const int daysBetweenShiftsCycle = 4;
            await AddShiftsToSchedule(cycle, firstDayShift, secondDayShift, daysBetweenShiftsCycle, startDate.Month);
        }

        private async Task ShiftsDayNight(DateTime startDate, string[] cycle, string firstShift)
        {
            IShift? dayShift = null;
            IShift? nightShift = null;

            if (firstShift == "DayShift")
            {
                dayShift = new DayShift("DayShift", startDate.Year, startDate.Month, startDate.Day);
                await this.schedule.AddShift(dayShift);
            }
            else if (firstShift == "NightShift")
            {
                nightShift = new NightShift("NightShift", startDate.Year, startDate.Month, startDate.Day);
                await this.schedule.AddShift(nightShift!);
            }

            if (dayShift == null)
            {
                dayShift = new DayShift("DayShift", startDate.Year, startDate.Month, startDate.Day - 1);
            }
            else if (nightShift == null)
            {
                nightShift = new NightShift("NightShift", startDate.Year, startDate.Month, startDate.Day + 1);
                await this.schedule.AddShift(nightShift!);
            }

            const int daysBetweenShiftsCycle = 4;
            await AddShiftsToSchedule(cycle, dayShift, nightShift!, daysBetweenShiftsCycle, startDate.Month);
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

                    this.schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDateTime.Year, tempDayDateTime.Month, tempDayDateTime.Day));

                    if (IsDayDayShift)
                    {
                        tempDayDateTime = tempDayDateTime.AddDays(1);

                        if (HasShiftMonthChanged(tempDayDateTime.Month, startDateMonth))
                            break;

                        this.schedule.AddShift(new DayShift(dayShift.ShiftType, tempDayDateTime.Year, tempDayDateTime.Month, tempDayDateTime.Day));
                    }
                }

                if (shift == "Night")
                {
                    tempNightDateTime = tempNightDateTime.AddDays(daysBetweenShiftsCycle);

                    if (HasShiftMonthChanged(tempNightDateTime.Month, startDateMonth))
                        break;

                    this.schedule.AddShift(new NightShift(nightShift.ShiftType, tempNightDateTime.Year, tempNightDateTime.Month, tempNightDateTime.Day));

                    if (IsDayNightNightShift)
                    {
                        tempSecondNightDateTime = tempSecondNightDateTime.AddDays(daysBetweenShiftsCycle);

                        if (HasShiftMonthChanged(tempSecondNightDateTime.Month, startDateMonth))
                            break;

                        this.schedule.AddShift(new NightShift(nightShift.ShiftType, tempSecondNightDateTime.Year, tempSecondNightDateTime.Month, tempSecondNightDateTime.Day));
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
