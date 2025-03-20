namespace WorkChronicle.Structure.Models
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

            sb.AppendLine($"{Day:d2}/{Month:d2}/{Year}"); // Can insert the shift hours or something else

            return sb.ToString().Trim();
        }

        public DateTime GetDateShift()
        {
            return new DateTime(Year, Month, Day);
        }
        public override bool Equals(object? obj)
        {
            if (obj is Shift otherShift)
            {
                return ShiftType == otherShift.ShiftType &&
                       Year == otherShift.Year &&
                       Month == otherShift.Month &&
                       Day == otherShift.Day &&
                       StarTime == otherShift.StarTime &&
                       ShiftHour == otherShift.ShiftHour &&
                       IsCompensated == otherShift.IsCompensated;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ShiftType,Year,Month,Day,StarTime,ShiftHour, IsCompensated);
        }

    }
}
