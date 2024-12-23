using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkChronicle.Logic.Models
{
    public class NightShift : Shift
    {
        public NightShift(DateTime dateShift) : base(dateShift)
        {
        }

        public override string ToString()
        {
            return $"Работиш нощни смени на: " + base.ToString() + $" до {DateShift.Hour - 12:d2}:{DateShift.Minute:d2}";
        }
    }
}
