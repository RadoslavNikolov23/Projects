namespace WorkChronicle.Core.Models
{
    public abstract class Shift : IShift
    {
        public Shift(ShiftType shiftType,int year, int month, int day, double startTime, double shiftHour)
        {
            ShiftType = shiftType;
            Year = year;
            Month = month;
            Day = day;
            StarTime = startTime;
            ShiftHour = shiftHour;
            ShiftType = shiftType;
            IsCompensated = false;
        }

        public ShiftType ShiftType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public double StarTime { get; set; }
        public double ShiftHour { get; set; }
        public bool IsCompensated { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{this.Day:d2}/{this.Month:d2}/{this.Year}"); // Can insert the shift hours or something else

            return sb.ToString().Trim();
        }

        public DateTime GetDateShift()
        {
            return new DateTime(this.Year, this.Month, this.Day);
        }
        public override bool Equals(object? obj)
        {
            if (obj is Shift otherShift)
            {
                return this.ShiftType == otherShift.ShiftType &&
                       this.Year == otherShift.Year &&
                       this.Month == otherShift.Month &&
                       this.Day == otherShift.Day &&
                       this.StarTime == otherShift.StarTime &&
                       this.ShiftHour == otherShift.ShiftHour &&
                       this.IsCompensated == otherShift.IsCompensated;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.ShiftType,this.Year,this.Month,this.Day,this.StarTime,this.ShiftHour, this.IsCompensated);
        }

    }
}
