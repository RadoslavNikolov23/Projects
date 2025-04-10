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

        public async Task<DbShift> GetSingleShifts(int scheduleId, int year, int month, int day)
        {
            await EnsureInitialized();

            return await dbContext.Database
                                    .Table<DbShift>()
                                    .FirstOrDefaultAsync(s => s.DbScheduleId == scheduleId 
                                                            && s.Year == year 
                                                            && s.Month == month 
                                                            && s.Day == day);
        }

        public async Task<IList<DbShift>> GetShiftsWithSchedule()
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


        //private readonly WorkScheduleDB dbContext;

        //public WorkShiftRepositoryDB(WorkScheduleDB dbContext)
        //{
        //    this.dbContext = dbContext;

        //}

        //private async Task EnsureInitialized() => await dbContext.Init();

        //public async Task<int> AddShift(ShiftDto shiftDto)
        //{
        //    await EnsureInitialized();

        //    DbShift shift = new DbShift
        //    {
        //        ShiftType = shiftDto.ShiftType,
        //        Year = shiftDto.Year,
        //        Month = shiftDto.Month,
        //        Day = shiftDto.Day,
        //        StarTime = shiftDto.StarTime,
        //        ShiftHour = shiftDto.ShiftHour,
        //        IsCurrentMonth = shiftDto.IsCurrentMonth,
        //        IsCompensated = shiftDto.IsCompensated
        //    };

        //    return await dbContext.Database
        //                                .InsertAsync(shift);
        //}

        //public async Task<List<ShiftDto>> GetAllShifts()
        //{
        //    await EnsureInitialized();

        //    var shifts = await dbContext.Database.Table<DbShift>()
        //              .ToListAsync();

        //    var shiftDtos = shifts.Select(s => new ShiftDto
        //                                        {
        //                                            ShiftType = s.ShiftType,
        //                                            Year = s.Year,
        //                                            Month = s.Month,
        //                                            Day = s.Day,
        //                                            StarTime = s.StarTime,
        //                                            ShiftHour = s.ShiftHour,
        //                                            IsCurrentMonth = s.IsCurrentMonth,
        //                                            IsCompensated = s.IsCompensated
        //                                        })
        //                           .ToList();

        //    return shiftDtos;
        //}

        //public async Task<List<ShiftDto>> GetShiftsForSchedule(int scheduleId)
        //{
        //    await EnsureInitialized();

        //    var shifts = await dbContext.Database.Table<DbShift>()
        //                               .Where(s => s.DbScheduleId == scheduleId)
        //                               .ToListAsync();

        //    var shiftDtos = shifts.Select(s => new ShiftDto
        //                                        {
        //                                            ShiftType = s.ShiftType,
        //                                            Year = s.Year,
        //                                            Month = s.Month,
        //                                            Day = s.Day,
        //                                            StarTime = s.StarTime,
        //                                            ShiftHour = s.ShiftHour,
        //                                            IsCurrentMonth = s.IsCurrentMonth,
        //                                            IsCompensated = s.IsCompensated
        //                                        })
        //                           .ToList();

        //    return shiftDtos;
        //}

        ////---------Commented out for now, as it is not used in the current implementation---------
        //public async Task<IList<DbShift>> GetShiftsWithSchedule()
        //{
        //    await EnsureInitialized();

        //    var shifts = await dbContext.Database
        //                                .Table<DbShift>()
        //                                .ToListAsync();


        //    foreach (var shift in shifts)
        //    {
        //        shift.DbSchedule = await dbContext.Database
        //                                        .FindAsync<DbSchedule>(shift.DbScheduleId);
        //    }
        //    return shifts;
        //}

        //public async Task<int> UpdateShift(ShiftDto shiftDto)
        //{
        //    await EnsureInitialized();

        //    DbShift shift = new DbShift
        //    {
        //        ShiftType = shiftDto.ShiftType,
        //        Year = shiftDto.Year,
        //        Month = shiftDto.Month,
        //        Day = shiftDto.Day,
        //        StarTime = shiftDto.StarTime,
        //        ShiftHour = shiftDto.ShiftHour,
        //        IsCurrentMonth = shiftDto.IsCurrentMonth,
        //        IsCompensated = shiftDto.IsCompensated
        //    };

        //    return await dbContext.Database
        //                                .UpdateAsync(shift);
        //}

        //public async Task<int> DeleteShift(ShiftDto shiftDto)
        //{
        //    await EnsureInitialized();

        //    DbShift shift = new DbShift
        //    {
        //        ShiftType = shiftDto.ShiftType,
        //        Year = shiftDto.Year,
        //        Month = shiftDto.Month,
        //        Day = shiftDto.Day,
        //        StarTime = shiftDto.StarTime,
        //        ShiftHour = shiftDto.ShiftHour,
        //        IsCurrentMonth = shiftDto.IsCurrentMonth,
        //        IsCompensated = shiftDto.IsCompensated
        //    };

        //    return await dbContext.Database.
        //                                DeleteAsync(shift);
        //}
    }
}
