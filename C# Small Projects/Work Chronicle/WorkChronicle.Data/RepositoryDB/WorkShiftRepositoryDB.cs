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
            await EnsureInitialized();

            return await dbContext.Database
                                        .InsertAsync(shift);
        }

        public async Task<List<DbShift>> GetAllShifts()
        {
            await EnsureInitialized();

            return await dbContext.Database
                                        .Table<DbShift>()
                                        .ToListAsync();
        }

        public async Task<List<DbShift>> GetShiftsForSchedule(int scheduleId)
        {
            await EnsureInitialized();

            return await dbContext.Database
                                    .Table<DbShift>()
                                    .Where(s => s.DbScheduleId == scheduleId)
                                    .ToListAsync();
        }

        public async Task<List<DbShift>> GetShiftsWithSchedule()
        {
            await EnsureInitialized();

            var shifts = await dbContext.Database
                                        .Table<DbShift>()
                                        .ToListAsync();
            
            
            foreach (var shift in shifts)
            {
                shift.DbSchedule = await dbContext.Database
                                                .FindAsync<DbSchedule>(shift.DbScheduleId);
            }
            return shifts;
        }

        // 🔹 Update a shift
        public async Task<int> UpdateShift(DbShift shift)
        {
            await EnsureInitialized();

            return await dbContext.Database
                                        .UpdateAsync(shift);
        }

        public async Task<int> DeleteShift(DbShift shift)
        {
            await EnsureInitialized();

            return await dbContext.Database.
                                        DeleteAsync(shift);
        }
    }
}
