namespace WorkChronicle.Structure.Models
{
    public class NightShift : Shift
    {
        private const int hourNightShift = 13;
        public NightShift(ShiftType shiftType, int year, int month, int day, double startTime, double shiftHour) : base(shiftType, year, month, day, startTime, shiftHour)
        {
        }

        public override string ToString()
        {
            return $"You work Night Shift at: " + base.ToString();
        }
    }
   
}
