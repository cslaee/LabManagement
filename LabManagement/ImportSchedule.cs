using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LabManagement
{
    static class ImportSchedule
    {

        static readonly bool debug = Constants.importScheduleDebug;
        //todo logic for row is a class
        //todo Insert Class | Course,Title, Credit, DefaultRoom, ClassType
        //todo Insert Semester name, year
        //todo Insert User
        //todo Insert Schedule | lookup ClassID, Insert ClassID, Section, SemesesterID, Days, StartTime, EndTime, InstructorID, RoomID,  Revision Date?, Revision Version
        //todo Does Class DefaultRoom match Schedule Room?
        //todo Does Class maxSections match Schedule Section?

        static public void GetExcelSchedule()
        {
            Regex coursePattern = new Regex(@"EE(\d{4})");
            Regex course_Number = new Regex("EE");
            string fileName = @"C:\Users\moberme\Documents\ArletteTestSchedule.xlsx";
             ExcelData ws = new ExcelData(fileName, 1);

            //for (int i = 1; i <= ws.rowCount; i++)
            //{
            //    for (int j = 1; j <= ws.colCount; j++)
            //    {
            //        System.Console.Write(ws.excelArray[i - 1, j - 1] +"_|_");
            //    }
            //    System.Console.WriteLine(" ");
            //}

            Boolean isCourse;
            string courseNumber;
            string section;
            string title;
            string credit;
            string faculty;
            string days;
            string startTime;
            string endTime;
            string room;

            for (int currentRow = 0; currentRow <= ws.rowCount - 1; currentRow++)
            {
                isCourse = coursePattern.IsMatch(ws.excelArray[currentRow, 0]);


                if (isCourse)
                {
                    courseNumber = Regex.Match(ws.excelArray[currentRow, 0], @"(\d{4})").Value;
                    section = Regex.Match(ws.excelArray[currentRow, 0], @"-(\d{2})").Groups[1].Value;
                    title = ws.excelArray[currentRow, 1].Trim();
                    credit = ws.excelArray[currentRow, 2].Trim();
                    faculty = ws.excelArray[currentRow, 3].Trim();
                    days = ws.excelArray[currentRow, 4].Trim();
                    startTime = ws.excelArray[currentRow, 4].Trim();
                    endTime = ws.excelArray[currentRow, 4].Trim();
                    room = ws.excelArray[currentRow, 5].Trim();

                    string outString = courseNumber + " " + section + " " + title + " " + credit + " " + faculty + " " + days + " " + startTime + " " + endTime + " " + room;


                   //                 var match = Regex.Match(ws.excelArray[currentRow, 0], pattern, RegexOptions.IgnoreCase);
                    
                    //courseNumber = course_Number.Match(ws.excelArray[currentRow, 0], "").Value;
                    //Regex.Replace(ws.excelArray[currentRow, 0], pattern, String.Empty)
                    Console.WriteLine(outString);
                }
                //                System.Console.WriteLine(" ");
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
