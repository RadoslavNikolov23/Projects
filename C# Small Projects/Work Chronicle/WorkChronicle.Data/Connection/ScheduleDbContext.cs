namespace WorkChronicle.Data.Connection
{
    using Microsoft.EntityFrameworkCore;
    using WorkChronicle.Data.Models;

    using static Connection.DatabaseConfiguration;

    public class ScheduleDbContext : DbContext
    {
        public string DbPath { get; }
        public ScheduleDbContext()
        {
            DbPath = DatabasePath;
        }

        public virtual DbSet<ShiftRecord> ShiftRecords { get; set; } = null!;
        public virtual DbSet<ScheduleRecord> ScheduleRecords { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
        // protected override void OnConfiguring(DbContextOptionsBuilder options)
       // => options.UseSqlite($"Data Source={DbPath}");
    }
}
