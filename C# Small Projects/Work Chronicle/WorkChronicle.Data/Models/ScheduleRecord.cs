namespace WorkChronicle.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [Comment("In the ScheduleRecord Table will be stored all of the shift in a given month.")]
    public class ScheduleRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        [Comment("The name of the Schedule for the given month - will be the \"Month(by name) Year(by number)\"")]
        public string ScheduleName { get; set; } = null!;

        public virtual ICollection<ShiftRecord> ShiftRecords { get; set; } = new HashSet<ShiftRecord>();
    }
}
