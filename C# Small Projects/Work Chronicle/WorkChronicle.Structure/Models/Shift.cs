namespace WorkChronicle.Structure.Models
{
    public abstract class Shift : IShift
    {
        public Shift(ShiftType shiftType,int year, int month, int day, double startTime, double shiftHour): this(shiftType, year, month, day, startTime, shiftHour, false, false)
        {
        }

        public Shift(ShiftType shiftType,int year, int month, int day, double startTime, double shiftHour, bool isCurrentMonth): this (shiftType, year, month, day, startTime, shiftHour, isCurrentMonth,false)
        {
        }

        public Shift(ShiftType shiftType,int year, int month, int day, double startTime, double shiftHour, bool isCurrentMonth, bool isCompensated)
        {
            this.ShiftType = shiftType;
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.StarTime = startTime;
            this.ShiftHour = shiftHour;
            this.ShiftType = shiftType;
            this.IsCompensated = isCompensated;
            this.IsCurrentMonth = isCurrentMonth; 
            this.BackgroundColor = !IsCurrentMonth? "LightGray"
                                                : isCompensated ? "LightBlue"
                                                : shiftType == ShiftType.DayShift ? "LightGreen"
                                                : shiftType == ShiftType.NightShift ? "DarkGreen" 
                                                : "White"; //For RestDay and everything else!
        }

        public ShiftType ShiftType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public double StarTime { get; set; }
        public double ShiftHour { get; set; }
        public bool IsCompensated { get; set; }
        public bool IsCurrentMonth { get; set; }
        public string BackgroundColor { get; set; }

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
