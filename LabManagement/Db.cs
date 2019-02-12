using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LabManagement
{
    class Db
    {
        static readonly bool debug = Constants.dbDebug;

        static public void StartDb()
        {
            IfWipeDbTrue();
            IfNotExistsCreateDatabase();
            // System.Environment.Exit(1);
        }


        public static string GetDbSchema()
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


        public static void IfNotExistsCreateDatabase()
        {
            //  https://github.com/tatsushid/mysql-wb-exportsqlite
            //--   Then export file.
            //--   Tools>Catalog>ExportSqliteTableCoumns"
            if (!File.Exists("./" + Constants.databaseName))
            {
                System.Console.WriteLine("No Database exsist, Creating one");
                SQLiteConnection.CreateFile(Constants.databaseName);
                System.Console.WriteLine("Building Tables");
                BuildDbTables(GetDbSchema());
                //todo Add PRAGMA foreign_keys=ON
                ImportExcelData();
            }
        }

        public static void IfWipeDbTrue()
        {
            if (Constants.wipeDB && File.Exists("./" + Constants.databaseName))
            {
                    File.Delete("./" + Constants.databaseName);
                    System.Console.WriteLine("Database file deleted");
            }
        }


        private static int BuildDbTables(string sqlStatement)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(Constants.connectionString))
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


        static public void ImportExcelData()
        {
            string fileName = System.AppContext.BaseDirectory + @"InitialData.xlsx";
            List<ExcelData> excelList = ExcelData.GetEntireWorkbook(fileName);
            foreach (ExcelData ws in excelList)
            {
                string[,] sheetData = new string[ws.rowCount - 1, ws.colCount];
                for (int currentRow = 1; currentRow <= ws.rowCount - 1; currentRow++)
                {
                    for (int currentColumn = 0; currentColumn <= ws.colCount -1; currentColumn++)
                    {
                        sheetData[currentRow - 1, currentColumn] = "'" + ws.excelArray[currentRow, currentColumn] + "'";
                    }
                }
            Console.WriteLine("ws.sheetName = " + ws.sheetName + "ws.sqlColumnString = " + ws.sqlColumnString + "sheetData = " +sheetData);
            Db.SqlInsertArray(ws.sheetName, ws.sqlColumnString, sheetData);
            }
        }


 
        static public List<string> GetID(string table, string idName, string id)
        {
            var returnString = new List<string>();
            var myObject = new object[100];
            SQLiteConnection connection = new SQLiteConnection(Constants.connectionString);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "select * from " + table + " where " + idName + " = " + id;
            connection.Open();

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        returnString.Add(reader.GetValue(i).ToString());
                }
            }
            connection.Close();
            return returnString;
        }

        static public int UpdateID(string table, string idName, string id, string colName, string colValue)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(Constants.connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    string comboQuery = "UPDATE " + table + " SET " + colName + " = '" + colValue + "' WHERE " + idName + " = " + id;
                    System.Console.WriteLine(comboQuery);
                    cmd.CommandText = comboQuery;

                    try
                    {
                        result = cmd.ExecuteNonQuery();
                        System.Console.WriteLine("Updated ID " + id);
                    }
                    catch (SQLiteException)
                    {
                        System.Console.WriteLine("SQLiteException Deleting ID " + id);
                    }
                }
                conn.Close();
            }
            return result;
        }


        static public int DeleteID(string table, string idName, string id)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(Constants.connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    string comboQuery = "DELETE FROM " + table + " WHERE " + idName + " = " + id;
                    System.Console.WriteLine(comboQuery);
                    cmd.CommandText = comboQuery;

                    try
                    {
                        result = cmd.ExecuteNonQuery();
                        System.Console.WriteLine("Deleted ID " + id);
                    }
                    catch (SQLiteException)
                    {
                        System.Console.WriteLine("SQLiteException Deleting ID " + id);
                    }
                }
                conn.Close();
            }
            return result;
        }


        static public int SqlInsert(string name, string column, string values)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(Constants.connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    string comboQuery = "INSERT INTO " + name + " (" + column + ") VALUES(" + values + ")";
                    System.Console.WriteLine(comboQuery);
                    cmd.CommandText = comboQuery;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        System.Console.WriteLine("Created Table");

                        cmd.CommandText = "SELECT LAST_INSERT_ROWID()";
                        //System.Console.WriteLine("Return ID = " + reader.GetValue(i).ToString());
                        //result = cmd.ExecuteNonQuery();
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            result = System.Int32.Parse(reader.GetValue(0).ToString());

                            //return reader["col_1"];
                        }
                    }
                    catch (SQLiteException)
                    {
                        System.Console.WriteLine("SQLiteException Creating table");
                    }
                }
                conn.Close();
            }
            return result;
        }


        static public int SqlInsertArray(string name, string column, string[,] values)
        {
            int result = -1;
            int numRow = values.GetLength(0);
            int numCol = values.GetLength(1);
            StringBuilder val = new StringBuilder();
            string queryLeft = "INSERT INTO " + name + " (" + column + ") VALUES(";
            int queryLeftLen = queryLeft.Length;
            val.Append(queryLeft);
            if (debug)
                System.Console.WriteLine("queryLeft = " + queryLeft);

            using (SQLiteConnection conn = new SQLiteConnection(Constants.connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    try
                    {
                        for (int j = 0; j < numRow; ++j)
                        {
                            for (int i = 0; i < numCol; ++i)
                            {
                                val.Append(values[j, i] + ", ");
                            }
                            val.Remove(val.Length - 2, 2);
                            val.Append(")");
                            cmd.CommandText = val.ToString();
                            if (debug)
                                System.Console.WriteLine("query = " + val.ToString());
                            result = cmd.ExecuteNonQuery();
                            val.Remove(queryLeftLen, val.Length - queryLeftLen);
                        }
                        System.Console.WriteLine("Finished Inserting Array into table");
                    }
                    catch (SQLiteException)
                    {
                        System.Console.WriteLine("SQLiteException Creating table");
                    }
                }
                conn.Close();
            }
            return result;
        }

        static public int SqlInsertObject(string name, string column, Lock[] locks)
        {
            int result = -1;
            int numRow = locks.Length;
            StringBuilder val = new StringBuilder();
            string queryLeft = "INSERT INTO " + name + " (" + column + ") VALUES(";
            int queryLeftLen = queryLeft.Length;
            val.Append(queryLeft);

            using (SQLiteConnection conn = new SQLiteConnection(Constants.connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    try
                    {
                        for (int j = 0; j < numRow; ++j)
                        {

                            val.Append(locks[j].id + ", " + locks[j].cw1 + ", " + locks[j].ccw + ", " + locks[j].cw2 + ")");
                            cmd.CommandText = val.ToString();
                            result = cmd.ExecuteNonQuery();
                            val.Remove(queryLeftLen, val.Length - queryLeftLen);
                        }
                        System.Console.WriteLine("Finished Creating Table");
                    }
                    catch (SQLiteException)
                    {
                        System.Console.WriteLine("SQLiteException Creating table");
                    }
                }
                conn.Close();
            }
            return result;
        }

    }

}
