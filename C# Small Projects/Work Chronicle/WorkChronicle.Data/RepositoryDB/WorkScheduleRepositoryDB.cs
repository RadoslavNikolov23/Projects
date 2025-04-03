namespace WorkChronicle.Data.RepositoryDB
{
    public class WorkScheduleRepositoryDB
    {

        private readonly WorkScheduleDB dbContext;

        public WorkScheduleRepositoryDB(WorkScheduleDB dbContext)
        {
            this.dbContext = dbContext;
        }


        public Task<DbSchedule> GetLastSchedule()
        {

            return dbContext.Database.Table<DbSchedule>()
                            .OrderByDescending(s => s.Id)
                            .FirstAsync(); // Assuming CreatedDate is a property of ScheduleRecord
        }



        private async Task EnsureInitialized() => await dbContext.Init();

        public async Task<int> AddSchedule(DbSchedule schedule)
        {
            await EnsureInitialized();

            return await dbContext.Database
                                        .InsertAsync(schedule);
        }

        public async Task<List<DbSchedule>> GetSchedules()
        {
            await EnsureInitialized();

            return await dbContext.Database
                                        .Table<DbSchedule>()
                                        .ToListAsync();
        }

        public async Task<int> UpdateSchedule(DbSchedule schedule)
        {
            await EnsureInitialized();

            return await dbContext.Database
                                        .UpdateAsync(schedule);
        }

        public async Task<int> DeleteSchedule(DbSchedule schedule)
        {
            await EnsureInitialized();

            await dbContext.Database
                                .ExecuteAsync("DELETE FROM DbShift WHERE ScheduleId = ?", schedule.Id);
           
            return await dbContext.Database
                                    .DeleteAsync(schedule);
        }





















        //private readonly WorkScheduleDB dbContext;

        //public ScheduleRepositoryDB(WorkScheduleDB dbContext)
        //{
        //    this.dbContext = dbContext;
        //    dbContext.Database.EnsureCreated();
        //}

        //public async Task SaveScheduleDB(DbSchedule scheduleRecord)
        //{
        //    dbContext.DbSchedules.Add(scheduleRecord);
        //    await dbContext.SaveChangesAsync();
        //}

        //public async Task<DbSchedule?> GetScheduleByNameAsync(string name)
        //{
        //    return await dbContext.DbSchedules
        //                    .FirstOrDefaultAsync(s => s.ScheduleName == name);
        //}

        //public async Task<List<DbSchedule>> GetSchedulesAsync()
        //{
        //    //TODO: Check if lazy loading is working, if not usi eager loading!   -----:> return await dbContext.ScheduleRecords.Include(s => s.ShiftRecords).ToListAsync();

        //    return await dbContext.DbSchedules.ToListAsync();
        //}

        //public async Task DeleteScheduleAsync(int scheduleId)
        //{
        //    //TODO: Check if lazy loading is working, if not usi eager loading! ---- >var schedule = await dbContext.ScheduleRecords.Include(s => s.ShiftRecords).FirstOrDefaultAsync(s => s.Id == scheduleId);

        //    var schedule = await dbContext.DbSchedules.FirstOrDefaultAsync(s => s.Id == scheduleId);

        //    if (schedule != null && schedule.ShiftRecords != null)
        //    {
        //        dbContext.DbShifts.RemoveRange(schedule.ShiftRecords);
        //        dbContext.DbSchedules.Remove(schedule);
        //        await dbContext.SaveChangesAsync();

        //    }

        //}
    }
}
