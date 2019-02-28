using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
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
                    for (int currentColumn = 0; currentColumn <= ws.colCount - 1; currentColumn++)
                    {
                        sheetData[currentRow - 1, currentColumn] = "'" + ws.excelArray[currentRow, currentColumn] + "'";
                    }
                }
                Console.WriteLine("ws.sheetName = " + ws.sheetName + "ws.sqlColumnString = " + ws.sqlColumnString + "sheetData = " + sheetData);
                Db.SqlInsertArray(ws.sheetName, ws.sqlColumnString, sheetData);
            }
        }

        static public long GetSingleInt(string table, string searchColumn, string matchString, string returnColumn)
        {
            var returnString = new List<string>();
            SQLiteConnection connection = new SQLiteConnection(Constants.connectionString);
            SQLiteCommand command = connection.CreateCommand();
            string sqlStr = "select " + returnColumn + " from " + table + " where " + searchColumn + " = " + matchString + " COLLATE NOCASE";
//sqlStr = select ' semesterNameID from SemesterName where name ' = FALL COLLATE NOCASE
            Console.WriteLine("sqlStr = " + sqlStr);
            command.CommandText = sqlStr; 
            connection.Open();
            long value = -1;

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                reader.Read();
                value = Convert.ToInt64(reader[returnColumn]);
            }

            connection.Close();
            return value;
        }


        static public string GetSingleString(string table, string searchColumn, string matchString, string returnColumn)
        {
            var returnString = new List<string>();
            SQLiteConnection connection = new SQLiteConnection(Constants.connectionString);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "select " + returnColumn + " from " + table + " where " + searchColumn + " = " + matchString + " COLLATE NOCASE";
            //Console.WriteLine("select " + returnColumn + " from " + table + " where " + searchColumn + " = " + matchValue + " COLLATE NOCASE");
            connection.Open();
            string value = "";
            try
            {
                value = (string)command.ExecuteScalar();
            }
            catch (SQLiteException)
            {
                System.Console.WriteLine("SQLiteException GetSingleValue ");
            }
            connection.Close();
            return value;
        }

        static public List<object> GetTuple(object obj, string searchString)
        {
            string tableName = Regex.Match(obj.ToString(), @"(\w+)\.(\w+)").Groups[2].Value;
            var returnString = new List<object>();
            SQLiteConnection connection = new SQLiteConnection(Constants.connectionString);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "select * from " + tableName + " where " + searchString + " COLLATE NOCASE";
            connection.Open();
            string type;

//            obj.GetType().GetProperty("year").SetValue(obj, 9999, null);// pretty cool
            Console.WriteLine("Start 3");

            Console.WriteLine("Start 3 ++ " + obj.ToString());
            foreach (var prop in obj.GetType().GetProperties())
            {
                Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(obj, null));
            }
            Console.WriteLine("End 3");



            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        //returnString.Add(reader.GetValue(i).ToString());
                        type = reader.GetDataTypeName(i);
                        switch (type)
                        {
                            case "INTEGER":
                                Console.WriteLine("found int");
                                //Convert.ToInt64()
                                returnString.Add(reader.GetValue(i).ToString());
                                //bool notNumeric = !int.TryParse(lockNumber, out int n);
                                break;
                            case "DATE":
                                Console.WriteLine("found date");
                                //returnString.Add(reader.GetValue(i).ToString());
                                break;
                        }
                        //returnString.Add(reader.GetValue(i));
                        //returnString.Add(reader.GetValue(i).ToString());
                        //Console.WriteLine("returnString = " + returnString[i]);
                    }
                }
            }
            connection.Close();
            return returnString;
        }







        static public List<string> GetTuple(string table, string searchColumn, string matchString)
        {
            var returnString = new List<string>();
            SQLiteConnection connection = new SQLiteConnection(Constants.connectionString);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "select * from " + table + " where " + searchColumn + " = " + matchString + " COLLATE NOCASE";
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

        static public int UpdateIDOld(string table, string idName, string id, string colName, string colValue)
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

        static public int UpdateID(string table, string idName, int id, string colNameAndValue)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(Constants.connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    string comboQuery = "UPDATE " + table + " SET " + colNameAndValue +  " WHERE " + idName + " = " + id;
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
