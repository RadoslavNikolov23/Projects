using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkChronicle.Structure.Models;

namespace WorkChronicle.Structure.Database
{
    public class ScheduleDatabase
    {
        private readonly SQLiteAsyncConnection database;

        public ScheduleDatabase()
        {
            var dbPath=Path.Combine(FileSystem.AppDataDirectory, "schedules.db3");
            database = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            database.CreateTableAsync<ShiftDTO>().Wait();
        }

        public Task<int> SaveShiftAsync(ShiftDTO shift)
        {
                return database.InsertAsync(shift);
           
        }

        public Task<int> DeleteShiftAsync(ShiftDTO shift)
        {
            return database.DeleteAsync(shift);
        }

        public Task<List<ShiftDTO>> GetShiftsAsync(int year, int month)
        {
            return database.Table<ShiftDTO>()
                .Where(i => i.Year == year && i.Month == month)
                .ToListAsync();
        }
    }
}
