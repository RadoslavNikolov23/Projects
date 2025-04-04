namespace WorkChronicle.Structure.Models
{
    public class RestDay : Shift
    {
        public RestDay(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour) : this(shiftType, year, month, day, startTime, shiftHour, false)
        {
        }

        public RestDay(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour, bool isCompensated) : base(shiftType, year, month, day, startTime, shiftHour, isCompensated)
        {
        }

        public override string ToString()
        {
            return $"Rest day: {Day:d2}/{Month:d2}/{Year}";
        }
    }
}
