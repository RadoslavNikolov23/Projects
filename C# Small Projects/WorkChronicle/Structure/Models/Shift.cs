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
        public Shift(string shiftType,int year, int month, int day, int hour)
        {
            ShiftType = shiftType;
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            ShiftType = shiftType;
            isCompensated = false;
            this.workShift = new DateTime(this.Year, this.Month, this.Day, this.Hour, 0, 0);
        }

        public string ShiftType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public bool isCompensated { get; set; }
        public DateTime WorkShift { get => this.workShift; }

        public DateTime GetDateShift()
        {
            return WorkShift;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{WorkShift.Day:d2}/{WorkShift.Month:d2}/{WorkShift.Year}");

            return sb.ToString().Trim();
        }
    }
}
