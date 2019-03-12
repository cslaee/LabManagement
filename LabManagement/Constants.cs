using System;

namespace LabManagement
{
    class Constants
    {
        public const bool wipeDB = false;
        public const bool dbDebug = false;
        public const bool importScheduleDebug = false;
        public const bool semesterDebug = false;
        public const bool courseDebug = false;
        public const bool roomDebug = true;

        public const string username = "John Doe";
        public const string email = "test@test.test";
        public const string databaseName = "CalStateLAeeDB.sqlite3";
        public const string connectionString = @"Data Source=|DataDirectory|" + databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";

        //    static string connectionString = @"Data Source=" + Constants.databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";
        public const string locksJsonFileName = "Locks.json";
        public const string sqlFileName = "db.sql";
        public string workingDirectory = System.AppContext.BaseDirectory;
    }
            // ** useful. Passed from a this obj
            //  obj.GetType().GetProperty("year").SetValue(obj, 9999, null);// pretty cool
            //foreach (var prop in obj.GetType().GetProperties())
            //{
            //    Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(obj, null));
            //}

}

