using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChronicle.Core.Models.Contracts;

namespace WorkChronicle.Core.Models
{
    public abstract class Shift : IShift
    {
        private DateTime workShift;
        public Shift(int year, int month, int day, int hour)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            this.workShift = new DateTime(this.Year, this.Month, this.Day, this.Hour, 0, 0);
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public DateTime WorkShift { get => this.workShift; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{WorkShift.Day:d2}/{WorkShift.Month:d2}/{WorkShift.Year}");

            return sb.ToString().Trim();
        }
    }
}
