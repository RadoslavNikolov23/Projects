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
        public List<string> CalculateShifts(DateTime startDate, string[] cycle)
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
            DateTime tempNightDateTime = new DateTime(startDate.Year,startDate.Month,startDate.Day+1);
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

                        if (dayShift.Month != startDate.Month)
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

                        if (nightShift.Month != startDate.Month)
                            break;

                        schedule.AddShift(nightShift);

                    }
                }
                   
            }

            List<string> shifts = new List<string>();

            foreach (var shift in schedule.WorkSchedule)
            {
                shifts.Add(shift.ToString());
            }

            return shifts;
        }
        public int CalculateTotalHours(List<string> shifts)
        {
            int totalHours = 0;
            foreach (var shift in shifts)
            {
                if (shift.Contains("Day"))
                {
                    totalHours += 12;
                }
                else if (shift.Contains("Night"))
                {
                    totalHours += 13;
                }
            }
            return totalHours;
        }

    }
}
