using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace LabManagement
{
    static class ImportSchedule
    {

    static readonly bool debug = Constants.importScheduleDebug;


        static public void GetExcelSchedule()
        {
            string fileName = GetFileName();







            ExcelToArray(fileName);

//            System.Environment.Exit(1);
        }



        static public string GetFileName()
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }
            return filePath;
        }




static void ExcelToArray(string fileName)
        {
            string[] workSheets = new string[3] { "Lock", "UserType", "User" };
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            string firstWorksheet = xlWorkSheet.Name.ToString();
            int[] lastUserRowAndColumn = ExcelHelper.GetLastColumnAndRow(xlWorkSheet);

            int lastUsedRow = xlWorkSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

            int lastUsedColumn = xlWorkSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;
           
            
            
            
            //foreach (string workSheetString in workSheets)
            //{
            //    ImportSheet(xlWorkBook, workSheetString);
            //}
            xlWorkBook.Close(true, null, null);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            //MessageBox.Show(fileName);
            MessageBox.Show(firstWorksheet + " last row =" +lastUserRowAndColumn[0] + "last col =" + lastUserRowAndColumn[1]);
        }




        static void ImportSheet(Excel.Workbook workBook, string workSheet)
        {
            Excel.Worksheet xlWorkSheet;
            xlWorkSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(workSheet);
            Excel.Range range;
            range = xlWorkSheet.UsedRange;

            int lastUsedRow = xlWorkSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

            int lastUsedColumn = xlWorkSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;
            //            System.Console.WriteLine("last row=" + lastUsedRow + "last column=" + lastUsedColumn);

            string str;
            StringBuilder sqlColumnString2 = new StringBuilder();
            object cellObject;
            string[,] sheetData = new string[lastUsedRow - 1, lastUsedColumn];
            string[] columnData = new string[lastUsedColumn];
            if (debug)
                System.Console.WriteLine("* Now inserting " + workSheet + " worksheet into tables");

            for (int currentColumn = 1; currentColumn <= lastUsedColumn; currentColumn++)
            {
                columnData[currentColumn - 1] = (string)(range.Cells[1, currentColumn] as Excel.Range).Value2;
                if (debug)
                    System.Console.WriteLine("columnData = " + columnData[currentColumn - 1]);
            }

            string sqlColumnString = string.Join(", ", columnData);
            if (debug)
                System.Console.WriteLine("columns = " + sqlColumnString);

            for (int currentRow = 2; currentRow <= lastUsedRow; currentRow++)
            {
                for (int currentColumn = 1; currentColumn <= lastUsedColumn; currentColumn++)
                {
                    cellObject = (range.Cells[currentRow, currentColumn] as Excel.Range).Value2;
                    str = Convert.ToString(cellObject);
                    sheetData[currentRow - 2, currentColumn - 1] = "'" + str + "'";
                    sqlColumnString2.Append(str + ", ");
                }
                if (debug)
                {
                    System.Console.WriteLine("sqlColumnString2 = " + sqlColumnString2);
                    sqlColumnString2.Clear();
                }

            }
            Marshal.ReleaseComObject(xlWorkSheet);

            Db.SqlInsertArray(workSheet, sqlColumnString, sheetData);
            if (debug)
                System.Console.WriteLine("* Finished inserting " + workSheet + " worksheet into tables");
        }










    }
}
