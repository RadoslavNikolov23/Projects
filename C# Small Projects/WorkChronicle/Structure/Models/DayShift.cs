using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Core.Models
{
    public class DayShift : Shift
    {
        private const int hourDayShift = 12;
        public DayShift(string shiftType,int year, int month, int day) : base (shiftType, year, month, day, hourDayShift)
        {
            
        }

        public override string ToString()
        {
            return $"You work Day Shift at: " + base.ToString();
        }
    }
   
}
