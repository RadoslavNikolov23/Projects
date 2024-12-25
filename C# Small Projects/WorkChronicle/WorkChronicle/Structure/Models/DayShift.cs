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
        public DayShift(int year, int month, int day) : base (year, month, day, hourDayShift)
        {
            
        }

        public override string ToString()
        {
            return $"Работиш дневна сменя на: " + base.ToString();
              //  + $" до {WorkShift.Hour + 12:d2}:{WorkShift.Minute:d2}";
        }
    }
   
}
