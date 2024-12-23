using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Logic.Models
{
    public class DayShift : Shift
    {
        public DayShift(DateTime dateShift) : base(dateShift)
        {
        }

        public override string ToString()
        {
            return $"Работиш дневни смени на: " + base.ToString() + $" до {DateShift.Hour + 12:d2}:{DateShift.Minute:d2}";
        }
    }
}
