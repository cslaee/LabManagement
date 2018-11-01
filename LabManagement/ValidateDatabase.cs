using System;
using System.Data.SQLite;
using System.IO;

namespace LabManagement
{

    public class ValidateDatabase
    {
        static string connectionString = @"Data Source=" + Constants.databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";

        static public void TestTables()
        {
            String[,] tables = new string[9, 2]{
           {"EventType",
                "description VARCHAR(45) PRIMARY KEY UNIQUE"},
           {"Calendar",
                "calendarID	INTEGER PRIMARY KEY AUTO_INCREMENT, subject VARCHAR(45), semesterFK INT, eventTypeFK VATCHAR(45), startDate DATE, endDate DATE, startTime DATETIME, endTime DATETIME"},
           {"Semester",
                "semesterID	INTEGER PRIMARY KEY, name VARCHAR(45), year INT"},
           {"Status",
                "description VARCHAR(45) PRIMARY KEY UNIQUE"},
           {"Schedule",
                "id	INTEGER PRIMARY KEY, cw1 INTEGER, ccw INTEGER, cw2 INTEGER"},
           {"TaughtBy",
                "id	INTEGER PRIMARY KEY, cw1 INTEGER, ccw INTEGER, cw2 INTEGER"},
           {"User",
                "id	INTEGER PRIMARY KEY, cw1 INTEGER, ccw INTEGER, cw2 INTEGER"},
           {"Lock",
                "id	INTEGER PRIMARY KEY, cw1 INTEGER, ccw INTEGER, cw2 INTEGER"},
           {"Source",
            "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE," +
            "LangTitle TEXT NOT NULL UNIQUE CHECK (LangTitle <> '')" }
           };

            Console.WriteLine("Check Databases");
            DeleteDatabaseFile();
            //        System.Threading.Thread.Sleep(1000);
            ValidateDatabaseFile();
            for (int i = 0; i < tables.GetLength(0); i++)
            {
                CreateTable(tables[i, 0], tables[i, 1]);
            }
            InitialData.Fill();
           // System.Environment.Exit(1);
        }


        public static void ValidateDatabaseFile()
        {
            if (!File.Exists("./" + Constants.databaseName))
            {
                SQLiteConnection.CreateFile(Constants.databaseName);
                System.Console.WriteLine("No Database exsist, file created");
            }
        }

        public static void DeleteDatabaseFile()
        {
            if (File.Exists("./" + Constants.databaseName))
            {
                File.Delete("./" + Constants.databaseName);
                System.Console.WriteLine("Database file deleted");
            }
        }


        public static void ValidateTable()
        {
            if (!File.Exists("./" + Constants.databaseName))
            {
                SQLiteConnection.CreateFile(Constants.databaseName);
                System.Console.WriteLine("No Database exsist, file created");
            }
        }

        private static int CreateTable(String name, String columns)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    String ctine = "CREATE TABLE IF NOT EXISTS ";
                    Console.WriteLine(ctine + name + " (" + columns + ")");
                    cmd.CommandText = @ctine + name + " (" + columns + ")";

                    try
                    {
                        result = cmd.ExecuteNonQuery();
                        System.Console.WriteLine("Created Table");
                    }
                    catch (SQLiteException)
                    {
                        Console.WriteLine("SQLiteException Creating table");

                    }
                }
                conn.Close();
            }
            return result;
        }

    }

}
