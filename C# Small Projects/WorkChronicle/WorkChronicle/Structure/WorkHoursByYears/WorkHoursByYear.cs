using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Structure.WorkHoursByYears
{
    public class WorkHoursByYear
    {
        public static KeyValuePair<int, string[]> Year2024(int month)
        {
            Dictionary<int, string[]> monthByWorkDaysByWorkHours = new Dictionary<int, string[]>()
            {
                { 1, new string[] { "22", "176" } },
                { 2, new string[] {  "21", "168" } },
                { 3, new string[] {  "20", "160" } },
                { 4, new string[] {  "22", "176" } },
                { 5, new string[] {  "19", "152"  } },
                { 6, new string[] { "20", "160",} },
                { 7, new string[] {  "23", "184",} },
                { 8, new string[] {  "22", "176" } },
                { 9, new string[] {  "19", "152" } },
                { 10, new string[] {  "23", "184" } },
                { 11, new string[] {  "21", "168" } },
                { 12, new string[] { "19", "152" }},
                { 13, new string[] { "251", "2008" }}
            };

            return monthByWorkDaysByWorkHours.FirstOrDefault(x => x.Key == month);
        }
    }
}
