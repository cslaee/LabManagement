using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace LabManagement
{
    class ExcelHelper
    {
        public int rowCount;
        public int colCount;
        public string[,] excelArray;
        public string sheetName;


        public void GetLastColumnAndRow(Excel.Worksheet xlWorkSheet)
        {
            rowCount = xlWorkSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;

            colCount = xlWorkSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                           System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                           Excel.XlSearchOrder.xlByColumns, Excel.XlSearchDirection.xlPrevious,
                                           false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;
        }


        public ExcelHelper(string fileName)  //Gets the first sheet
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            sheetName = xlWorkSheet.Name.ToString();
            Excel.Range range;
            range = xlWorkSheet.UsedRange;
            GetLastColumnAndRow(xlWorkSheet);
            excelArray = new string[rowCount, colCount];

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    excelArray[i - 1, j - 1] = range.Cells[i, j].Text;
                }
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }



        public ExcelHelper(Excel.Worksheet xlWorkSheet)
        {
            Excel.Range range;
            range = xlWorkSheet.UsedRange;
            GetLastColumnAndRow(xlWorkSheet);
            excelArray = new string[rowCount, colCount];

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    string str = range.Cells[i, j].Text;
                    excelArray[i - 1, j - 1] = str;
                }
            }
        }

    }
}
