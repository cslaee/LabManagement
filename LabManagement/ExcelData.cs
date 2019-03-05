using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Excel = Microsoft.Office.Interop.Excel;  //new


namespace LabManagement
{
    class ExcelData
    {
        public int rowCount;
        public int colCount;
        public string[,] excelArray;
        public string[] firstRowData;
        public string sqlColumnString;
        public string sheetName;


        public void GetLastColumnAndRow(Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet)
        {
            rowCount = xlWorkSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

            colCount = xlWorkSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Microsoft.Office.Interop.Excel.XlSearchOrder.xlByColumns, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;
        }


        public ExcelData(string fileName, int sheetNumber)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbooks xlWorkbooks = xlApp.Workbooks;
            Excel.Workbook xlWorkbook = xlWorkbooks.Open(fileName, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkbook.Worksheets.get_Item(sheetNumber);
            sheetName = xlWorkSheet.Name.ToString();
            Excel.Range range;
            range = xlWorkSheet.UsedRange;
            GetLastColumnAndRow(xlWorkSheet);
            excelArray = new string[rowCount, colCount];
            firstRowData = new string[colCount];
            for (int j = 1; j <= colCount; j++)
                firstRowData[j - 1] = range.Cells[1, j].Text;
            sqlColumnString = string.Join(", ", firstRowData);


            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    //excelArray[i - 1, j - 1] = range.Cells[i, j].Text;

                        excelArray[i - 1, j - 1] = range.Cells[i, j].Text;
                    //if (range.Cells[i, j] != null && range.Cells[i, j].Value2 != null)
                    //{
                    //   // System.Console.Write(range.Cells[i, j].Value2.ToString() + "\t");
                    //    excelArray[i - 1, j - 1] = range.Cells[i, j].Text;

                    //}
                    //else
                    //{

                    //    excelArray[i - 1, j - 1] = "Null from ExcelData()";
                    //}
                }
            }

            
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(range);
            Marshal.ReleaseComObject(xlWorkSheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlWorkbooks);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }

        public ExcelData(Microsoft.Office.Interop.Excel.Workbook xlWorkBook, Microsoft.Office.Interop.Excel.Application xlApp, int sheetNumber)
        {
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(sheetNumber);
            sheetName = xlWorkSheet.Name.ToString();
            Microsoft.Office.Interop.Excel.Range range;
            range = xlWorkSheet.UsedRange;
            GetLastColumnAndRow(xlWorkSheet);
            excelArray = new string[rowCount, colCount];
            firstRowData = new string[colCount];

            for (int j = 1; j <= colCount; j++)
                firstRowData[j - 1] = range.Cells[1, j].Text;
            sqlColumnString = string.Join(", ", firstRowData);


            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    excelArray[i - 1, j - 1] = range.Cells[i, j].Text;
                }
            }
        }


        static void OpenWorkbook(Microsoft.Office.Interop.Excel.Workbook xlWorkBook, Microsoft.Office.Interop.Excel.Application xlApp)
        {
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            //            xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        }

        static void CloseWorkbook(Microsoft.Office.Interop.Excel.Workbook xlWorkBook, Microsoft.Office.Interop.Excel.Application xlApp)
        {
            xlWorkBook.Close(true, null, null);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

        }




        static public List<ExcelData> GetEntireWorkbook(string fileName)
        {
            List<ExcelData> excelList = new List<ExcelData>();

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            int numberOfWorksheets = xlWorkBook.Sheets.Count;

            for (int currentWorksheet = 1; currentWorksheet <= numberOfWorksheets; currentWorksheet++)
            {
                excelList.Add(new ExcelData(xlWorkBook, xlApp, currentWorksheet));
            }
            CloseWorkbook(xlWorkBook, xlApp);
            return excelList;
        }


    }
}
