using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;

namespace LabManagement
{
    class InitialData
    {
        static readonly bool debug = Constants.initialDataDebug;
        static public int[,] lockCombo = new int[10, 4] {
             {1, 2, 3, 4}, {2, 3, 4, 5}, {3, 4, 5, 6}, {4, 5, 6, 7}, {5, 6, 7, 8}, {6, 7, 8, 9}, {7, 8, 9, 10}, {8, 9, 10, 11}, {9, 10, 11, 12}, {10, 11, 12, 13}
                        };

        //static public var[,] lockerType = new var[1, 4] {{1,1,1,1}};

        static public void Fill()
        {
            // Db.InsertRows("Lock", "id, cw1, ccw, cw2", lockCombo);
            Db.SaveArrayToJson(lockCombo);
            string locksFile = System.AppContext.BaseDirectory + Constants.locksJsonFileName;
            Console.WriteLine("dir =" + locksFile);
            Lock[] MasterLocks = JsonConvert.DeserializeObject<Lock[]>(File.ReadAllText(locksFile));
            Db.SqlInsertObject("Lock", "id, cw1, ccw, cw2", MasterLocks);

            string displayableVersion = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
            Console.WriteLine("Version = " + displayableVersion);
        }

        static public void ImportExcelData()
        {
            //string[] workSheets = new string[2] { "Lock", "User" };
            string[] workSheets = new string[1] { "User" };

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            //string fileLocation = @"C:\Users\moberme\source\repos\ImportExcelDemo\importMe.xls";
            string fileToImport = System.AppContext.BaseDirectory + @"InitialData.xlsx";
            // MessageBox.Show(fileToImport);
            xlApp = new Excel.Application();
            string fileLocation = fileToImport;
            xlWorkBook = xlApp.Workbooks.Open(fileLocation, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            foreach (string workSheetString in workSheets)
            {
                ImportSheet(xlWorkBook, workSheetString);
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }

        static void ImportSheet(Excel.Workbook workBook, string workSheet)
        {
            Excel.Worksheet xlWorkSheet;
            xlWorkSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(workSheet);
            Excel.Range range;
            range = xlWorkSheet.UsedRange;
            int rw = range.Rows.Count;
            int cl = range.Columns.Count;
            string str;
            StringBuilder sqlColumnString2 = new StringBuilder();
            sqlColumnString2.Append("'");
            int rCnt;
            int cCnt;
            double cellInSheet;
            string[,] sheetData = new string[rw - 1, cl];
            string[] columnData = new string[cl];
                    if (debug)
                        System.Console.WriteLine("Now importing " + workSheet + " worksheet");
 
            for (cCnt = 1; cCnt <= cl; cCnt++)
            {
                columnData[cCnt - 1] = (string)(range.Cells[1, cCnt] as Excel.Range).Value2;
            }

            string sqlColumnString = string.Join(", ", columnData);
            if (debug)
                System.Console.WriteLine("columns = " + sqlColumnString);

            //     MessageBox.Show("start>" + sqlColumnString);

            for (rCnt = 2; rCnt <= rw; rCnt++)
            {
                for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    str = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value2;
//                    str = "'5'"; 
                    sheetData[rCnt - 2, cCnt - 1] = "'" + str + "'";
                    sqlColumnString2.Append(str + "',");
//                    if (debug)
//                        System.Console.WriteLine("columns2 = " + sqlColumnString2);
                        //System.Console.WriteLine("columns2 = " + sqlColumnString2);
                }
                if (debug)
                {
                    System.Console.WriteLine("sqlColumnString2 = " + sqlColumnString2);
                    sqlColumnString2.Clear();
                }

            }
            Marshal.ReleaseComObject(xlWorkSheet);

            Db.SqlInsertArray(workSheet, sqlColumnString, sheetData);
        }
    }
}
