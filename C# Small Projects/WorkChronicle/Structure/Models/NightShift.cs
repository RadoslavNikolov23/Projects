namespace WorkChronicle.Core.Models
{
    public class NightShift : Shift
    {
        private const int hourNightShift = 13;
        public NightShift(string shiftType,int year, int month, int day) : base(shiftType, year, month, day, hourNightShift)
        {
        }

        public override string ToString()
        {
            return $"You work Night Shift at: " + base.ToString();
        }
    }
   
}
