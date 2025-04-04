namespace WorkChronicle.Data.Models
{
    public class DbSchedule
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        [MaxLength(15)]
        public string ScheduleName { get; set; } = null!;

        public int Year { get; set; }

        public int Month { get; set; }

        [Ignore]
        public virtual ICollection<DbShift> ShiftRecords { get; set; } = new HashSet<DbShift>();
    }
}
