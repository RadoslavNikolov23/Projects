using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Core.Models
{
    public class NightShift : Shift
    {
        private const int hourNightShift = 13;
        public NightShift(int year, int month, int day) : base(year, month, day, hourNightShift)
        {
        }

        public override string ToString()
        {
            return $"Работиш нощна сменя на: " + base.ToString();
            //+ $" до {WorkShift.Hour - 12:d2}:{WorkShift.Minute:d2}";
        }
    }
   
}
