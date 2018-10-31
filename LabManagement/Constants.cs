using System;

namespace LabManagement
{
    class Constants
    {
        public const string username = "John Doe";
        public const string email = "test@test.test";
        public const String databaseName = "CalStateLAeeDB.sqlite3";
        public const string connectionString = @"Data Source=|DataDirectory|" + databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";
        public const string locksJsonFileName = "Locks.json";
        public string workingDirectory = System.AppContext.BaseDirectory;

    }


}

