namespace WorkChronicle.Structure.Models
{
    public class RestDay : Shift
    {
        public RestDay(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour) : base(shiftType, year, month, day, startTime, shiftHour, false, false)
        {
        }

        public RestDay(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour, bool isCurrentMonth) : base (shiftType, year, month, day, startTime, shiftHour, isCurrentMonth, false)
        {
        }



        public RestDay(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour, bool isCurrentMonth, bool isCompensated) : base (shiftType, year, month, day, startTime, shiftHour, isCurrentMonth, isCompensated)
        {
        }

        public override string ToString()
        {
            return $"Rest day: {Day:d2}/{Month:d2}/{Year}";
        }
    }
}
