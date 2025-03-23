namespace WorkChronicle.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using WorkChronicle.Structure.Models.Enums;

    [Comment("Shift Table, that contains all of the shift information")]
    public class ShiftRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Comment("The type of the current shift, can be DayShift/NightShift")]
        public ShiftTypes ShiftType { get; set; }

        [Required]
        [Comment("The year of the current shift")]
        public int Year { get; set; }

        [Required]
        [Comment("The month of the current shift")]
        public int Month { get; set; }

        [Required]
        [Comment("The day of the current shift")]
        public int Day { get; set; }

        [Required]
        [Comment("The starting time of the current shift")]
        public double StarTime { get; set; }

        [Required]
        [Comment("The total hours of work during the current shift")]
        public double ShiftHour { get; set; }

        [Required]
        [Comment("The option to compensate the current shift/or not")]
        public bool IsCompensated { get; set; }

        [Required]
        [ForeignKey(nameof(ScheduleRecord))]
        public int ScheduleRecordId { get; set; }

        public virtual ScheduleRecord ScheduleRecord { get; set; } = null!;
    }

}
