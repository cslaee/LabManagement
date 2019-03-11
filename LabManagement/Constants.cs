using System;

namespace LabManagement
{
    class Constants
    {
        public const bool wipeDB = false;
        public const bool dbDebug = true;
        public const bool importScheduleDebug = false;
        public const bool semesterDebug = true;
        public const bool courseDebug = true;

        public const string username = "John Doe";
        public const string email = "test@test.test";
        public const string databaseName = "CalStateLAeeDB.sqlite3";
        public const string connectionString = @"Data Source=|DataDirectory|" + databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";

        //    static string connectionString = @"Data Source=" + Constants.databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";
        public const string locksJsonFileName = "Locks.json";
        public const string sqlFileName = "db.sql";
        public string workingDirectory = System.AppContext.BaseDirectory;
    }


}

