using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LabManagement
{

    public class ValidateDatabase
    {
        static string connectionString = @"Data Source=" + Constants.databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";

        static public void TestTables()
        {
            DeleteDatabaseFile();
            ValidateDatabaseFile();
            string schema = ReadDbSchema();
            BuildTables(ReadDbSchema());
            InitialData.Fill();
            // System.Environment.Exit(1);
        }


        public static string ReadDbSchema()
        {
            string sqlFile = System.AppContext.BaseDirectory + Constants.sqlFileName;
            string pattern = @"""mydb""" + @"\.|ATTACH(?:.*?);|BEGIN;|COMMIT;";
            string lines;
            using (var streamReader = File.OpenText(sqlFile))
            {
                lines = streamReader.ReadToEnd();
            }
            return Regex.Replace(lines, pattern, String.Empty);
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



        private static int BuildTables(String sqlStatement)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = sqlStatement;

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
