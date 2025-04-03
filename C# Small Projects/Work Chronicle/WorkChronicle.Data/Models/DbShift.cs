namespace WorkChronicle.Data.Models
{


    public class DbShift
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        public ShiftTypes ShiftType { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public double StarTime { get; set; }

        public double ShiftHour { get; set; }

        public bool IsCompensated { get; set; }

        public int DbScheduleId { get; set; }

        [Ignore]
        public virtual DbSchedule DbSchedule { get; set; } = null!;
    }

}
