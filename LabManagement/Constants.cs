using System;

namespace LabManagement
{
    class Constants
    {
        public const bool wipeDB = false;
        public const bool dbDebug = true;
        public const bool importScheduleDebug = false;
        public const bool semesterDebug = false;
        public const bool courseDebug = false;
        public const bool roomDebug = false;
        public const bool schedule = false;

        public const string username = "John Doe";
        public const string email = "test@test.test";
        public const string databaseName = "CalStateLAeeDB.sqlite3";
        public const string connectionString = @"Data Source=|DataDirectory|" + databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";

        //    static string connectionString = @"Data Source=" + Constants.databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";
        public const string locksJsonFileName = "Locks.json";
        public const string sqlFileName = "db.sql";
        public string workingDirectory = System.AppContext.BaseDirectory;
        public const string webpageDir = @"C:\Users\moberme\Documents\LabManagement\webpage\index_files\";
    }
            // ** useful. Passed from a this obj
            //  obj.GetType().GetProperty("year").SetValue(obj, 9999, null);// pretty cool
            //foreach (var prop in obj.GetType().GetProperties())
            //{
            //    Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(obj, null));
            //}

}
//todo Add Edit User Panel
//todo Add Edit Course Panel
//todo Add Edit Room Panel
//todo Build Send email to instructor button

        //todo Does Class DefaultRoom match Schedule Room?
        //todo Does Class maxSections match Schedule Section?


