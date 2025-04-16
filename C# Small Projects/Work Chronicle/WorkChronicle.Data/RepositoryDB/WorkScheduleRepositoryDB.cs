namespace WorkChronicle.Data.RepositoryDB
{
    public class WorkScheduleRepositoryDB
    {
        private readonly WorkScheduleDB dbContext;

        public WorkScheduleRepositoryDB(WorkScheduleDB dbContext)
        {
            this.dbContext = dbContext;
        }

        private async Task EnsureInitialized() => await dbContext.Init();

        public async Task<DbSchedule> GetLastSchedule()
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database.Table<DbSchedule>()
                                .OrderByDescending(s => s.Id)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetLastSchedule, in the WorkScheduleRepository class.");
                throw;
            }
        }

        public async Task<DbSchedule> GetScheduleByName(string name)
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database.Table<DbSchedule>()
                                .FirstOrDefaultAsync(s => s.ScheduleName == name);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetScheduleByName, in the WorkScheduleRepository class.");
                throw;
            }
        }

        public async Task<List<string>> GetAllScheduleNames()
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database
                                      .QueryScalarsAsync<string>
                                      ("SELECT ScheduleName FROM DbSchedule ORDER BY Year, Month");
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in GetAllScheduleNames, in the WorkScheduleRepository class.");
                throw;
            }
        }

        public async Task<int> AddSchedule(DbSchedule schedule)
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database
                                            .InsertAsync(schedule);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in AddSchedule, in the WorkScheduleRepositoryDb class.");
                throw;
            }


        }

        public async Task<bool> ExistsSchedule(string scheduleName)
        {
            try
            {

                await EnsureInitialized();

                int result = await dbContext.Database.FindWithQueryAsync<int>
                                            ("SELECT COUNT(*) FROM DbSchedule WHERE ScheduleName = ?", scheduleName);

                return result > 0;

            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in Exits, in the WorkScheduleRepositoryDb class.");
                throw;
            }

        }


        public async Task<int> UpdateSchedule(DbSchedule schedule)
        {
            try
            {
                await EnsureInitialized();

                return await dbContext.Database
                                            .UpdateAsync(schedule);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in UpdateSchedule, in the WorkScheduleRepositoryDb class.");
                throw;
            }
        }

        public async Task<int> DeleteSchedule(DbSchedule schedule)
        {

            try
            {
                await EnsureInitialized();

                await dbContext.Database
                                    .ExecuteAsync("DELETE FROM DbShift WHERE DbScheduleId = ?", schedule.Id);

                return await dbContext.Database
                                        .DeleteAsync(schedule);
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in DeleteSchedule,in the WorkScheduleRepositoryDb class.");
                throw;
            }


           
        }
    }
}
