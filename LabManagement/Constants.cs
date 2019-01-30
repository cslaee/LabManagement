﻿using System;

namespace LabManagement
{
    class Constants
    {
        public const Boolean deleteDB = true;
        public const Boolean dbDebug = false;
        public const Boolean initialDataDebug = true;

        public const string username = "John Doe";
        public const string email = "test@test.test";
        public const String databaseName = "CalStateLAeeDB.sqlite3";
        public const string connectionString = @"Data Source=|DataDirectory|" + databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";
        public const string locksJsonFileName = "Locks.json";
        public const string sqlFileName = "db.sql";
        public string workingDirectory = System.AppContext.BaseDirectory;

    }


}

