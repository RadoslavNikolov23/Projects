using CalendarShiftDemo.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarShiftDemo.Models
{
    public abstract class Shift : IShift
    {

        public Shift(DateTime dateShift)
        {
            this.DateShift = dateShift;
        }

        public DateTime DateShift { get; set; }

       
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{DateShift.Day:d2}/{DateShift.Month:d2}/{DateShift.Year} " +
                $"- от {DateShift.Hour:d2}:{DateShift.Minute:d2} ");
            
            return sb.ToString().Trim();
        }
    }
}
