namespace WorkChronicle.Structure.Models
{
    public abstract partial class Shift : ObservableObject, IShift
    {
        [ObservableProperty]
        private ShiftType shiftType;

        [ObservableProperty]
        private int year;

        [ObservableProperty]
        private int month;

        [ObservableProperty]
        private int day;

        [ObservableProperty]
        private double starTime;

        [ObservableProperty]
        private double shiftHour;

        [ObservableProperty]
        private bool isCompensated;

        [ObservableProperty]
        private bool isCurrentMonth;

        [ObservableProperty]
        private Color backgroundColor;



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
            this.IsCurrentMonth = isCurrentMonth;
            this.IsCompensated = isCompensated;
            this.BackgroundColor = !IsCurrentMonth ? Colors.LightGray
                                                : IsCompensated ? Colors.LightBlue
                                                : ShiftType == ShiftType.DayShift ? Colors.LightGreen
                                                : ShiftType == ShiftType.NightShift ? Colors.YellowGreen
                                                : Colors.White; //For RestDay and everything else!
        }

        partial void OnShiftTypeChanged(ShiftType value)
        {
            UpdateBackgroundColor();
        }

        partial void OnIsCompensatedChanged(bool value)
        {
            UpdateBackgroundColor();
        }

        partial void OnIsCurrentMonthChanged(bool value)
        {
            UpdateBackgroundColor();
        }

        private void UpdateBackgroundColor()
        {
            if (!IsCurrentMonth)
                BackgroundColor = Colors.LightGray;
            else if (IsCompensated)
                BackgroundColor = Colors.LightBlue;
            else if (ShiftType == ShiftType.DayShift)
                BackgroundColor = Colors.LightGreen;
            else if (ShiftType == ShiftType.NightShift)
                BackgroundColor = Colors.YellowGreen;
            else
                BackgroundColor = Colors.White;
        }

        public DateTime GetDateShift()
        {
            return new DateTime(Year, Month, Day);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{Day:d2}/{Month:d2}/{Year}"); // Can insert the shift hours or something else

            return sb.ToString().Trim();
        }

        //-----Check if these method are needed or not!-----//
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
                       IsCurrentMonth == otherShift.IsCurrentMonth &&
                       BackgroundColor == otherShift.BackgroundColor &&
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
