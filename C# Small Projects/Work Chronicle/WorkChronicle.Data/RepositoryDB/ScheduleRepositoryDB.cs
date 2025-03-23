namespace WorkChronicle.Data.RepositoryDB
{
    using Microsoft.EntityFrameworkCore;
    using WorkChronicle.Data.Connection;
    using WorkChronicle.Data.Models;

    public class ScheduleRepositoryDB
    {
        private readonly ScheduleDbContext dbContext;

        public ScheduleRepositoryDB(ScheduleDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Database.EnsureCreated();
        }

        public async Task SaveScheduleDB(ScheduleRecord scheduleRecord)
        {
            dbContext.ScheduleRecords.Add(scheduleRecord);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ScheduleRecord?> GetScheduleByNameAsync(string name)
        {
            return await dbContext.ScheduleRecords
                            .FirstOrDefaultAsync(s => s.ScheduleName == name);
        }

        public async Task<List<ScheduleRecord>> GetSchedulesAsync()
        {
            //TODO: Check if lazy loading is working, if not usi eager loading!   -----:> return await dbContext.ScheduleRecords.Include(s => s.ShiftRecords).ToListAsync();

            return await dbContext.ScheduleRecords.ToListAsync();
        }

        public async Task DeleteScheduleAsync(int scheduleId)
        {
            //TODO: Check if lazy loading is working, if not usi eager loading! ---- >var schedule = await dbContext.ScheduleRecords.Include(s => s.ShiftRecords).FirstOrDefaultAsync(s => s.Id == scheduleId);

            var schedule = await dbContext.ScheduleRecords.FirstOrDefaultAsync(s => s.Id == scheduleId);

            if (schedule != null && schedule.ShiftRecords != null)
            {
                dbContext.ShiftRecords.RemoveRange(schedule.ShiftRecords);
                dbContext.ScheduleRecords.Remove(schedule);
                await dbContext.SaveChangesAsync();

            }

        }
    }
}
