namespace WorkChronicle.Structure.Models
{
    public class DayShift : Shift
    {
       // private const int hourDayShift = 12;
        public DayShift(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour) : base(shiftType, year, month, day, startTime, shiftHour)
        {

        }

        public override string ToString()
        {
            return $"You work Day Shift at: " + base.ToString();
        }
    }
   
}
