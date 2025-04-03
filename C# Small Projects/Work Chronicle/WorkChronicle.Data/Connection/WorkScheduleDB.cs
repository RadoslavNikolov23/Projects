namespace WorkChronicle.Data.Connection
{
   


    using static Connection.DatabaseConfiguration;

    public class WorkScheduleDB 
    {
        private SQLiteAsyncConnection? database;

        public async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(DatabasePath, Flags);

            await database.CreateTableAsync<DbSchedule>();
            await database.CreateTableAsync<DbShift>();
        }

        public SQLiteAsyncConnection Database => database!;
       
    }

}
