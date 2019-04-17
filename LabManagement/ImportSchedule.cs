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
            Regex summerSessionABCRegex = new Regex(Constants.summerSessionABCPattern);

            int[,] revisionDateSearchPath = new int[,] { { 1, 0 }, { 2, 0 } };
            int[,] seasonSearchPath = new int[,] { { 1, 0 }, { 1, 1 }, { 2, 0 } };
            int[,] semesterDateRangeSearchPath = new int[,] { { 0, 0 }, { 1, 1 }, { 2, 0 } };

            // 2 digit dates not valid
            Common.DebugWriteLine(debug, "");
            Common.DebugWriteLine(debug, "GetExcelSchedule() fileName = " + fileName);
            ExcelData ws = new ExcelData(fileName, 1);
            int numberOfWsRows = ws.rowCount - 1;

            string revisionDateString = FindString(revisionDateSearchPath, ws, revisionDateRegex);
            string semesterNameAndYear = FindString(seasonSearchPath, ws, semesterNameAndYearRegex);
            bool notValidSemesterNameAndYear = semesterNameAndYear.Length == 0;

            if (notValidSemesterNameAndYear)
            {
                MessageBox.Show("Import Failed.  Can not find semester name and date.");
            }


            string semesterName = semesterNameAndYearRegex.Match(semesterNameAndYear).Groups[1].Value;
            string semesterYear = semesterNameAndYearRegex.Match(semesterNameAndYear).Groups[2].Value;
            long semesterNameFK = 0;
            //long semesterNameFK = Db.GetTupleInt("SemesterName", "semesterNameID", "name", semesterName);



            string[] colname = new[] { "name" };
            var coldata = new object[] { semesterName };
            var tuple = Db.GetTuple("SemesterName", "semesterNameID", colname, coldata);

            bool hasSemesterNameFK = tuple.Count > 0;
            if (hasSemesterNameFK)
            {
                semesterNameFK = Convert.ToInt32(tuple[0].ToString());
            }







            isSummer = semesterNameFK == 4;
            if (isSummer)
            {

                Common.DebugWriteLine(debug, "******  This is summer    *****");
                List<int> sessionRows = FindSummerSessions(ws, numberOfWsRows, summerSessionABCRegex);

                Common.DebugWriteLine(debug, "found semester that has sessions ABC" + sessionRows.Count);
                int numberOfSessionRows = sessionRows.Count;
                bool hasABCSessions = numberOfSessionRows > 0;

                if (hasABCSessions)
                {
                    for (int currentSession = 0; currentSession < numberOfSessionRows - 1; currentSession++)
                    {
                        int firstRowOfSession = sessionRows[currentSession];
                        int lastRowOfSession = sessionRows[currentSession + 1];

                        string summerSessionDateRange = ws.excelArray[firstRowOfSession, 1].Trim();
                        string session = summerSessionABCRegex.Match(summerSessionDateRange).Groups[1].Value;
                        string weeks = summerSessionABCRegex.Match(summerSessionDateRange).Groups[2].Value;
                        string startMonth = summerSessionABCRegex.Match(summerSessionDateRange).Groups[4].Value;
                        string startDay = summerSessionABCRegex.Match(summerSessionDateRange).Groups[5].Value;
                        string endMonth = summerSessionABCRegex.Match(summerSessionDateRange).Groups[7].Value;
                        string endDay = summerSessionABCRegex.Match(summerSessionDateRange).Groups[8].Value;
                        string output = session + weeks + startMonth + startDay + endMonth + endDay;
                        Common.DebugWriteLine(debug, summerSessionDateRange);
                        Common.DebugWriteLine(debug, output);

                        for (int rows = firstRowOfSession + 1; rows < lastRowOfSession; rows++)
                        {
                            Common.DebugWriteLine(debug, rows + " = " + ws.excelArray[rows, 1]);

                        }

                        colname = new[] { "name", "session", "numberOfWeeks" };
                        coldata = new object[] { "Summer", session, weeks };
                        tuple = Db.GetTuple("SemesterName", "semesterNameID", colname, coldata);
                        hasSemesterNameFK = tuple.Count > 0;
                        if (hasSemesterNameFK)
                        {
                            semesterNameFK = Convert.ToInt32(tuple[0].ToString());
                            Common.DebugWriteLine(debug, "semesterNameFK =" + semesterNameFK);
                        }

                        // ************************** Start Here  **************

                        Semester semester = new Semester(revisionDateString, semesterName, semesterYear, semesterNameFK);
                        Schedule.DeleteSchedule(semester.SemesterID);

                        //string semesterDateRange = FindString(semesterDateRangeSearchPath, ws, semesterDateRangeRegex);
                        //Common.DebugWriteLine(debug, "semesterDateRangeRegex = " + semesterDateRange);
                        //isValidSemesterDateRange = semesterDateRange.Length > 0;
                        //if (isValidSemesterDateRange)
                        //{
                        calendar = new Calendar(summerSessionDateRange, semester);

                    string yearStr = semester.Year.ToString();
                    string startDateStr = yearStr + Common.GetMonthDayString(summerSessionABCRegex, summerSessionDateRange, 4);
                    string endDateStr = yearStr + Common.GetMonthDayString(summerSessionABCRegex, summerSessionDateRange, 7);
                    calendar = new Calendar(startDateStr, endDateStr, semester);




                        //}
                        BuildSchedule(ws, semester.SemesterID, firstRowOfSession + 1, lastRowOfSession);

                        // ************************** End Here  **************

                    }
                }
                else
                {
                    Common.DebugWriteLine(debug, "******  This is summer Old   *****");
                }
            }
            else
            {
                Semester semester = new Semester(revisionDateString, semesterName, semesterYear, semesterNameFK);
                Schedule.DeleteSchedule(semester.SemesterID);

                string semesterDateRange = FindString(semesterDateRangeSearchPath, ws, semesterDateRangeRegex);
                Common.DebugWriteLine(debug, "semesterDateRangeRegex = " + semesterDateRange);
                isValidSemesterDateRange = semesterDateRange.Length > 0;
                if (isValidSemesterDateRange)
                {
                    //calendar = new Calendar(semesterDateRange, semester, 1);

                    string yearStr = semester.Year.ToString();
                    string startDateStr = yearStr + Common.GetMonthDayString(semesterDateRangeRegex, semesterDateRange, 1);
                    string endDateStr = yearStr + Common.GetMonthDayString(semesterDateRangeRegex, semesterDateRange, 5);
                    calendar = new Calendar(startDateStr, endDateStr, semester);
                }
                BuildSchedule(ws, semester.SemesterID, 4, numberOfWsRows);
            }
        }



        static List<int> FindSummerSessions(ExcelData ws, int numberOfWsRows, Regex summerSessionABCRegex)
        {
            List<int> summerSessionLineNumber = new List<int>();

            for (int row = 0; row < numberOfWsRows; row++)
            {
                string excelData = ws.excelArray[row, 1].Trim();
                string summerSessionDateRange = summerSessionABCRegex.Match(excelData).Groups[0].Value;
                bool hasDateRange = summerSessionDateRange.Length != 0;
                if (hasDateRange)
                {
                    summerSessionLineNumber.Add(row);
                }
            }

            bool isABCsessionSummer = summerSessionLineNumber.Count > 0;
            if (isABCsessionSummer)
            {
                //summerSessionLineNumber.Add(numberOfWsRows + 1);
                summerSessionLineNumber.Add(numberOfWsRows);
            }
            return summerSessionLineNumber;
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


        static void BuildSchedule(ExcelData ws, int semesterId, int firstRow, int lastRow)
        {
            Regex courseRegex = new Regex(Constants.coursePattern);
            Common.DebugWriteLine(debug, "semesterId = " + semesterId + " firstRow = " + firstRow + " lastRow = " + lastRow);

            for (int currentRow = firstRow; currentRow <= lastRow; currentRow++)
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
