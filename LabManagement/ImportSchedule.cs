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

            ExcelData x = new ExcelData(fileName, 1);

            for (int i = 1; i <= x.rowCount; i++)
            {
                for (int j = 1; j <= x.colCount; j++)
                {
                    System.Console.Write(x.excelArray[i - 1, j - 1] +"_|_");
                }
                System.Console.WriteLine(" ");
            }

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



    }
}
