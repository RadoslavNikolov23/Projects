namespace WorkChronicle.Structure.Models
{
    public class DayShift : Shift
    {
        public DayShift(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour) : base(shiftType, year, month, day, startTime, shiftHour, false, false)
        {
        }

        public DayShift(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour, bool isCurrentMonth) : base(shiftType, year, month, day, startTime, shiftHour, isCurrentMonth, false)
        {
        }

        public DayShift(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour, bool isCurrentMonth, bool isCompensated) : base(shiftType, year, month, day, startTime, shiftHour, isCurrentMonth, isCompensated)
        {
        }

        public override string ToString()
        {
            return $"You work Day Shift at: " + base.ToString();
        }
    }
   
}
