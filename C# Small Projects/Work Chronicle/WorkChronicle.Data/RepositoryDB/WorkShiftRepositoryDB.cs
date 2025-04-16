namespace WorkChronicle.Data.RepositoryDB
{
    public class WorkShiftRepositoryDB
    {
        private readonly WorkScheduleDB dbContext;

        public WorkShiftRepositoryDB(WorkScheduleDB dbContext)
        {
            this.dbContext = dbContext;
        }

        private async Task EnsureInitialized() => await dbContext.Init();

        public async Task<int> AddShift(DbShift shift)
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database
                                            .InsertAsync(shift);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in AddSchedule, in the WorkShiftRepositoryDB class.");
                throw;
            }
        }

        public async Task<List<DbShift>> GetAllShifts()
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database
                                            .Table<DbShift>()
                                            .ToListAsync();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetAllShifts, in the WorkShiftRepositoryDB class.");
                throw;
            }
        }

        public async Task<List<DbShift>> GetShiftsForSchedule(int scheduleId)
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database
                                        .Table<DbShift>()
                                        .Where(s => s.DbScheduleId == scheduleId)
                                        .ToListAsync();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetShiftsForSchedule, in the WorkShiftRepositoryDB class.");
                throw;
            }
        }

        public async Task<DbShift> GetSingleShifts(int scheduleId, int year, int month, int day)
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database
                                        .Table<DbShift>()
                                        .FirstOrDefaultAsync(s => s.DbScheduleId == scheduleId
                                                                && s.Year == year
                                                                && s.Month == month
                                                                && s.Day == day);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetSingleShifts, in the WorkShiftRepositoryDB class.");
                throw;
            }
        }

        public async Task<int> UpdateShift(DbShift shift)
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database
                                            .UpdateAsync(shift);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in UpdateShift, in the WorkShiftRepositoryDB class.");
                throw;
            }
        }

        public async Task<int> DeleteShift(DbShift shift)
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database.
                                            DeleteAsync(shift);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in DeleteShift, in the WorkShiftRepositoryDB class.");
                throw;
            }
        }
    }
}
