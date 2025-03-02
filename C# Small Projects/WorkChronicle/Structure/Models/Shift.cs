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
            IsCompensated = false;
            this.workShift = new DateTime(this.Year, this.Month, this.Day, this.Hour, 0, 0);
        }

        public string ShiftType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public bool IsCompensated { get; set; }
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

        public override bool Equals(object? obj)
        {
            if (obj is Shift otherShift)
            {
                return this.ShiftType == otherShift.ShiftType &&
                       this.Year == otherShift.Year &&
                       this.Month == otherShift.Month &&
                       this.Day == otherShift.Day &&
                       this.Hour == otherShift.Hour &&
                       this.IsCompensated == otherShift.IsCompensated &&
                       this.WorkShift == otherShift.WorkShift;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.ShiftType,this.Year,this.Month,this.Day,this.Hour,this.IsCompensated,this.WorkShift);
        }

    }
}
