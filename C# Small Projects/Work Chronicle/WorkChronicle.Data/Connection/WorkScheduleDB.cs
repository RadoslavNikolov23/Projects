namespace WorkChronicle.Data.Connection
{
    public class WorkScheduleDB 
    {
        private SQLiteAsyncConnection? database;

        public async Task Init()
        {
            try
            {
                if (database is not null)
                    return;

                database = new SQLiteAsyncConnection(DatabasePath, Flags);

                //---------For Deleting the tables and starting over for test purpose only!!-----------
                // await database.DropTableAsync<DbSchedule>();
                // await database.DropTableAsync<DbShift>();

                await database.CreateTableAsync<DbSchedule>();
                await database.CreateTableAsync<DbShift>();

                await database.ExecuteAsync("PRAGMA foreign_keys = ON;");
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in Init, in the WorkScheduleDBClass ");
                throw;
            }
        }

        public SQLiteAsyncConnection Database => database!;
       
    }
}
