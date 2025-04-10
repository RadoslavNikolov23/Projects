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
            await EnsureInitialized();

            return await dbContext.Database.Table<DbSchedule>()
                            .OrderByDescending(s => s.Id)
                            .FirstOrDefaultAsync();
        }

        public async Task<DbSchedule> GetScheduleByName(string name)
        {
            await EnsureInitialized();

            return await dbContext.Database.Table<DbSchedule>()
                            .FirstOrDefaultAsync(s => s.ScheduleName == name);
        }

        public async Task<List<string>> GetAllScheduleNames()
        {
            await EnsureInitialized();

            return await dbContext.Database
                                  .QueryScalarsAsync<string>
                                  ("SELECT ScheduleName FROM DbSchedule ORDER BY Year, Month");
        }

        public async Task<int> AddSchedule(DbSchedule schedule)
        {
            await EnsureInitialized();

            return await dbContext.Database
                                        .InsertAsync(schedule);
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

        //public WorkScheduleRepositoryDB(WorkScheduleDB dbContext)
        //{
        //    this.dbContext = dbContext;
        //}

        //private async Task EnsureInitialized() => await dbContext.Init();

        //public async Task<ScheduleDto> GetLastSchedule()
        //{
        //    await EnsureInitialized();

        //    var schedule = await dbContext.Database.Table<DbSchedule>()
        //                    .OrderByDescending(s => s.Id)
        //                    .FirstOrDefaultAsync();

        //    if (schedule == null)
        //        return null!;

        //    ScheduleDto scheduleDto = new ScheduleDto
        //    {
        //        Id = schedule.Id,
        //        ScheduleName = schedule.ScheduleName,
        //        Year = schedule.Year,
        //        Month = schedule.Month
        //    };


        //    ////IList<DbShift> shifts = await dbContext.Database.Table<DbShift>()
        //    ////                .Where(s => s.DbScheduleId == scheduleDto.Id)
        //    ////                .ToListAsync();

        //    //WorkShiftRepositoryDB shiftsRepo = new WorkShiftRepositoryDB(dbContext);

        //    //var shifts = await shiftsRepo.GetShiftsForSchedule(scheduleDto.Id);

        //    //scheduleDto.ShiftRecords = shifts.Select(s => new ShiftDto
        //    //                                        {
        //    //                                            ShiftType = s.ShiftType,
        //    //                                            Year = s.Year,
        //    //                                            Month = s.Month,
        //    //                                            Day = s.Day,
        //    //                                            StarTime = s.StarTime,
        //    //                                            ShiftHour = s.ShiftHour,
        //    //                                            IsCurrentMonth = s.IsCurrentMonth,
        //    //                                            IsCompensated = s.IsCompensated
        //    //                                        })
        //    //                                .ToList();

        //    return scheduleDto;
        //}

        //public async Task<ScheduleDto> GetScheduleByName(string name)
        //{
        //    await EnsureInitialized();

        //    var schedule = await dbContext.Database
        //                                .Table<DbSchedule>()
        //                                .FirstAsync(s => s.ScheduleName == name);


        //    if (schedule == null)
        //        return null!;

        //    var scheduleDto = new ScheduleDto
        //    {
        //        Id = schedule.Id,
        //        ScheduleName = schedule.ScheduleName,
        //        Year = schedule.Year,
        //        Month = schedule.Month
        //    };


        //    var shifts = await dbContext.Database.Table<DbShift>()
        //                    .Where(s => s.DbScheduleId == scheduleDto.Id)
        //                    .ToListAsync();

        //    scheduleDto.ShiftRecords = shifts.Select(s => new ShiftDto
        //                                            {
        //                                                ShiftType = s.ShiftType,
        //                                                Year = s.Year,
        //                                                Month = s.Month,
        //                                                Day = s.Day,
        //                                                StarTime = s.StarTime,
        //                                                ShiftHour = s.ShiftHour,
        //                                                IsCurrentMonth = s.IsCurrentMonth,
        //                                                IsCompensated = s.IsCompensated
        //                                            })
        //                                    .ToList();

        //    return scheduleDto;
        //}

        //public async Task<IList<string>> GetAllScheduleNames()
        //{
        //    await EnsureInitialized();


        //    return await dbContext.Database
        //                          .QueryScalarsAsync<string>
        //                          ("SELECT ScheduleName FROM DbSchedule ORDER BY Year, Month");
        //}

        //public async Task<int> AddSchedule(ScheduleDto scheduleDto)
        //{
        //    await EnsureInitialized();

        //    DbSchedule schedule = new DbSchedule
        //                    {
        //                        ScheduleName = scheduleDto.ScheduleName,
        //                        Year = scheduleDto.Year,
        //                        Month = scheduleDto.Month
        //                    };

        //    return await dbContext.Database
        //                                .InsertAsync(schedule);
        //}


        //public async Task<int> UpdateSchedule(ScheduleDto scheduleDto)
        //{
        //    await EnsureInitialized();

        //    DbSchedule schedule = new DbSchedule
        //                        {
        //                            Id = scheduleDto.Id,
        //                            ScheduleName = scheduleDto.ScheduleName,
        //                            Year = scheduleDto.Year,
        //                            Month = scheduleDto.Month
        //                        };

        //    return await dbContext.Database
        //                                .UpdateAsync(schedule);
        //}

        //public async Task<int> DeleteSchedule(ScheduleDto scheduleDto)
        //{
        //    await EnsureInitialized();

        //    DbSchedule schedule = new DbSchedule
        //    {
        //        Id = scheduleDto.Id,
        //        ScheduleName = scheduleDto.ScheduleName,
        //        Year = scheduleDto.Year,
        //        Month = scheduleDto.Month
        //    };

        //    await dbContext.Database
        //                        .ExecuteAsync("DELETE FROM DbShift WHERE ScheduleId = ?", schedule.Id);

        //    return await dbContext.Database
        //                            .DeleteAsync(schedule);
        //}
    }
}
