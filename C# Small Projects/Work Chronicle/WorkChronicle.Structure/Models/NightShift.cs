namespace WorkChronicle.Structure.Models
{
    public class NightShift : Shift
    {
        private const int hourNightShift = 13;

        public NightShift(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour) : this(shiftType, year, month, day, startTime, shiftHour, false)
        {
        }

        public NightShift(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour, bool isCompensated) : base(shiftType, year, month, day, startTime, shiftHour, isCompensated)
        {
        }

        public override string ToString()
        {
            return $"You work Night Shift at: " + base.ToString();
        }
    }
   
}
