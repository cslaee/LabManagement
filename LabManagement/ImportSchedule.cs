using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

        //todo If Fall, Spring or Winter semester
        //todo     Find date range for semester
        //todo  If Summer
        //todo    is it Session A B or C
        //todo    Find Date range.
 
namespace LabManagement
{
    static class ImportSchedule
    {
        static readonly bool debug = Constants.importScheduleDebug;

       static public void GetExcelSchedule(string fileName)
        {
            Calendar calendar;
            bool isSummer = false;
            bool isValidSemesterDateRange = false;
            Regex revisionDateRegex = new Regex(Constants.revisionDatePattern);
            Regex semesterNameAndYearRegex = new Regex(Constants.semesterNameAndYearPattern);
            Regex dayYearRegex = new Regex(Constants.dayYearPattern);
            Regex semesterDateRangeRegex = new Regex(Constants.semesterDateRangePattern);

            int[,] revisionDateSearchPath = new int[,] { { 1, 0 }, { 2, 0 } };
            int[,] seasonSearchPath = new int[,] { { 1, 0 }, { 1, 1 }, { 2, 0 } };
            int[,] semesterDateRangeSearchPath = new int[,] { { 0, 0 }, { 1, 1 }, { 2, 0 } };

            // 2 digit dates not valid
            Common.DebugWriteLine(debug, "");
            Common.DebugWriteLine(debug, "GetExcelSchedule() fileName = " + fileName);
            ExcelData ws = new ExcelData(fileName, 1);

            string revisionDateString = FindString(revisionDateSearchPath, ws, revisionDateRegex);
            string semesterNameAndYear = FindString(seasonSearchPath, ws, semesterNameAndYearRegex);
            bool notValidSemesterNameAndYear = semesterNameAndYear.Length == 0;

            if (notValidSemesterNameAndYear)
            {
                MessageBox.Show("Import Failed.  Can not find semester name and date.");
            }

            Semester semester = new Semester(revisionDateString, semesterNameAndYear);
            Schedule.DeleteSchedule(semester.SemesterID);

            isSummer = semester.NameFK == 4;
            if (isSummer)
            {
                Common.DebugWriteLine(debug, "This is summer");
            }
            else
            {
                string semesterDateRange = FindString(semesterDateRangeSearchPath, ws, semesterDateRangeRegex);
                Common.DebugWriteLine(debug, "semesterDateRangeRegex = " + semesterDateRange);
                isValidSemesterDateRange = semesterDateRange.Length > 0;
                if (isValidSemesterDateRange)
                {
                    calendar = new Calendar(semesterDateRange, semester, 1);
                }
                BuildSchedule(ws, semester.SemesterID, ws.rowCount - 1);
            }
        }


        static public string FindString(int[,] path, ExcelData ws, Regex pattern)
        {
            int numPaths = path.Length / 2;
            for (int y = 0; y < numPaths; y++)
            {
                string rawInput = ws.excelArray[path[y, 0], path[y, 1]].Trim();
                string excelString = pattern.Match(rawInput).Groups[0].Value;
                //    Console.WriteLine("NumPaths =" +numPaths + " rawInput =" + rawInput);
                bool hasWhatWeAreLookingFor = excelString.Length != 0;
                if (hasWhatWeAreLookingFor)
                {
                    return excelString;
                }

            }
            return "";
        }


        static void BuildSchedule(ExcelData ws, int semesterId, int lastRow)
        {
            Regex courseRegex = new Regex(Constants.coursePattern);
            Common.DebugWriteLine(debug, "semesterId = " + semesterId);

            for (int currentRow = 4; currentRow <= lastRow; currentRow++)
            {
                string rawCourse = ws.excelArray[currentRow, 0];
                bool isCourse = courseRegex.IsMatch(rawCourse);

                if (isCourse)
                {
                    string title = ws.excelArray[currentRow, 1].Trim();
                    string credit = ws.excelArray[currentRow, 2].Trim();
                    Course c = new Course(rawCourse, title, credit);
                    string coarseAndSection = c.Catalog + "-" + c.Section;
                    User u1 = new User(1, ws, currentRow);
                    User u2 = new User(2, ws, currentRow);
                    Room r1 = new Room(0, ws, currentRow);
                    Room r2 = new Room(5, ws, currentRow);
                    string rawTime = ws.excelArray[currentRow, 4].Trim();
                    Schedule s = new Schedule(c, semesterId, u1.UserID, u2.UserID, r1.RoomID, r2.RoomID, rawTime);
                }
            }

        }


        static public void TestImportSemesters()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2015");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2016");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2017");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2018");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2019");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\spring 2016");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\spring 2017");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\spring 2018");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\spring 2019");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\summer 2016");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\summer 2017");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\summer 2018");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\summer 2019");
            GetExcelSchedule(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\winter 2016");
            watch.Stop();
            Console.WriteLine("Time elapsed as per stopwatch: {0} ", watch.Elapsed);
        }


    }
}
