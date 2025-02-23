namespace WorkChronicle.Core.Models.Contracts
{
    public interface IShift
    {
        public string ShiftType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public bool isCompensated { get; set; }
        public DateTime WorkShift { get; }

        public DateTime GetDateShift();
    }
}
