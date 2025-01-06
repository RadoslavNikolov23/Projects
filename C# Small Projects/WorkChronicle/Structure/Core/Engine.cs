using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChronicle.Structure.Core.Contracts;
using Microsoft.Maui.Controls;
using WorkChronicle.Core.Repository.Contracts;
using WorkChronicle.Core.Models.Contracts;
using WorkChronicle.Core.Repository;
using WorkChronicle.Core.Models;

namespace WorkChronicle.Structure.Core
{
    public class Engine : IEngine
    {
        public ISchedule<IShift> CalculateShifts(DateTime startDate, string[] cycle)
        {
            if (cycle.Length == 2)
            {
                if (cycle[1] == "Night")
                {
                    return ShiftsDayNight(startDate, cycle);
                }
                else if (cycle[1] == "Day")
                {
                    return ShiftsDayDay(startDate, cycle);
                }
            }

            return ShiftsDayNightNight(startDate, cycle);
        }

        private ISchedule<IShift> ShiftsDayNightNight(DateTime startDate, string[] cycle)
        {
            ISchedule<IShift> schedule = new Schedule();

            IShift dayShift = new DayShift(startDate.Year, startDate.Month, startDate.Day);
            IShift firstNightShift = new NightShift(startDate.Year, startDate.Month, startDate.Day + 1);
            IShift secondNightShift = new NightShift(startDate.Year, startDate.Month, startDate.Day + 2);
            schedule.AddShift(dayShift);
            schedule.AddShift(firstNightShift);
            schedule.AddShift(secondNightShift);


            DateTime tempDayDateTime = startDate;
            DateTime tempFistNightDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day + 1);
            DateTime tempSecondNightDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day + 2);

            int counter = 0;

            while (true)
            {
                string shift = cycle[counter++ % 2];

                if (shift == "Day")
                {
                    if (dayShift != null)
                    {
                        tempDayDateTime = tempDayDateTime.AddDays(5);
                        dayShift = new DayShift(tempDayDateTime.Year, tempDayDateTime.Month, tempDayDateTime.Day);

                        if (HasShiftMonthChanged(dayShift, startDate))
                            break;

                        schedule.AddShift(dayShift);
                    }
                }

                if (shift == "Night")
                {
                    if (tempFistNightDateTime != null && tempSecondNightDateTime != null)
                    {
                        tempFistNightDateTime = tempFistNightDateTime.AddDays(5);
                        firstNightShift = new NightShift(tempFistNightDateTime.Year, tempFistNightDateTime.Month, tempFistNightDateTime.Day);

                        tempSecondNightDateTime = tempSecondNightDateTime.AddDays(5);
                        secondNightShift = new NightShift(tempSecondNightDateTime.Year, tempSecondNightDateTime.Month, tempSecondNightDateTime.Day);

                        if (HasShiftMonthChanged(firstNightShift, startDate) || tempSecondNightDateTime.Month != startDate.Month)
                            break;

                        schedule.AddShift(firstNightShift);
                        schedule.AddShift(secondNightShift);
                    }
                }
            }

            return schedule;
        }

        private ISchedule<IShift> ShiftsDayDay(DateTime startDate, string[] cycle)
        {
            ISchedule<IShift> schedule = new Schedule();

            IShift firstDayShift = new DayShift(startDate.Year, startDate.Month, startDate.Day);
            IShift secondDayShift = new DayShift(startDate.Year, startDate.Month, startDate.Day + 1);
            schedule.AddShift(firstDayShift);
            schedule.AddShift(secondDayShift);



            DateTime tempFirstDayDateTime = startDate;
            DateTime tempSecondDayDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day + 1);
            int counter = 0;

            while (true)
            {
                string shift = cycle[counter++ % cycle.Length];

                if (shift == "Day")
                {
                    if (firstDayShift != null)
                    {
                        tempFirstDayDateTime = tempFirstDayDateTime.AddDays(4);
                        firstDayShift = new DayShift(tempFirstDayDateTime.Year, tempFirstDayDateTime.Month, tempFirstDayDateTime.Day);

                        if (HasShiftMonthChanged(firstDayShift, startDate))
                            break;

                        schedule.AddShift(firstDayShift);
                    }

                    if (secondDayShift != null)
                    {
                        tempSecondDayDateTime = tempSecondDayDateTime.AddDays(4);
                        secondDayShift = new DayShift(tempSecondDayDateTime.Year, tempSecondDayDateTime.Month, tempSecondDayDateTime.Day);

                        if (HasShiftMonthChanged(secondDayShift, startDate))
                            break;

                        schedule.AddShift(secondDayShift);
                    }
                }
            }

            return schedule;
        }

        private ISchedule<IShift> ShiftsDayNight(DateTime startDate, string[] cycle)
        {
            ISchedule<IShift> schedule = new Schedule();

            IShift dayShift = null;
            IShift nightShift = null;

            if (cycle.Contains("Day"))
            {
                dayShift = new DayShift(startDate.Year, startDate.Month, startDate.Day);
                schedule.AddShift(dayShift);
            }
            if (cycle.Contains("Night"))
            {
                nightShift = new NightShift(startDate.Year, startDate.Month, startDate.Day + 1);
                schedule.AddShift(nightShift);
            }

            DateTime tempDayDateTime = startDate;
            DateTime tempNightDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day + 1);
            int counter = 0;

            while (true)
            {
                string shift = cycle[counter++ % cycle.Length];

                if (shift == "Day")
                {
                    if (dayShift != null)
                    {
                        tempDayDateTime = tempDayDateTime.AddDays(4);
                        dayShift = new DayShift(tempDayDateTime.Year, tempDayDateTime.Month, tempDayDateTime.Day);

                        if (HasShiftMonthChanged(dayShift, startDate))
                            break;

                        schedule.AddShift(dayShift);
                    }
                }

                if (shift == "Night")
                {
                    if (nightShift != null)
                    {
                        tempNightDateTime = tempNightDateTime.AddDays(4);
                        nightShift = new NightShift(tempNightDateTime.Year, tempNightDateTime.Month, tempNightDateTime.Day);

                        if (HasShiftMonthChanged(nightShift, startDate))
                            break;

                        schedule.AddShift(nightShift);

                    }
                }
            }

            return schedule;
        }

        public int CalculateTotalHours(ISchedule<IShift> schedule)
        {
            int totalHours = schedule.WorkSchedule.Sum(s => s.Hour);

            return totalHours;
        }

        private static bool HasShiftMonthChanged(IShift dayShift, DateTime startDate)
        {
            return dayShift.Month != startDate.Month;
        }

    }
}
