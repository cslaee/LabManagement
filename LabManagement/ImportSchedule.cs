using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace LabManagement
{
    static class ImportSchedule
    {
        static readonly bool debug = Constants.importScheduleDebug;
        static Regex courseRegex = new Regex(Constants.coursePattern);
        static Regex userRegex = new Regex(Constants.userPattern);
        static Regex roomRegex = new Regex(Constants.roomPattern);


        static public void GetExcelSchedule(string fileName)
        {
            Common.DebugWriteLine(debug, "GetExcelSchedule() fileName = " + fileName);
            ExcelData ws = new ExcelData(fileName, 1);

            string rawSemester = ws.excelArray[2, 0].Trim();
            Semester semester = new Semester(rawSemester);

            string[] colname = new[] { "semesterFK" };
            var coldata = new object[] { semester.SemesterID };
            Db.Delete("Schedule", colname, coldata);

            for (int currentRow = 4; currentRow <= ws.rowCount - 1; currentRow++)
            {
                string rawCourse = ws.excelArray[currentRow, 0];
                bool isCourse = courseRegex.IsMatch(rawCourse);

                if (isCourse)
                {
                    string title = ws.excelArray[currentRow, 1].Trim();
                    string credit = ws.excelArray[currentRow, 2].Trim();
                    Course c = new Course(rawCourse, title, credit);

                    string coarseAndSection = c.Catalog + "-" + c.Section;
                    User u1, u2;
                    string rawUser = ws.excelArray[currentRow, 3].Trim();
                    string user1 = userRegex.Match(rawUser).Groups[1].Value;
                    string user2 = userRegex.Match(rawUser).Groups[2].Value;
                    bool hasFirstUser = user1.Length != 0;
                    bool hasSecondUser = user2.Length != 0;
                    if (hasFirstUser)
                    {
                        u1 = new User(user1);
                    }
                    else
                    {
                        u1 = new User();
                    }
                    if (hasSecondUser)
                    {
                        u2 = new User(user2);
                    }
                    else
                    {
                        u2 = new User();
                    }
                    string users = u1.Last;

                    string rawRoom = ws.excelArray[currentRow, 5].Trim();
                    Room r1, r2;
                    string room1 = roomRegex.Match(rawRoom).Groups[1].Value;
                    string room2 = roomRegex.Match(rawRoom).Groups[6].Value;
                    bool hasFirstRoom = room1.Length != 0;
                    bool hasSecondRoom = room2.Length != 0;
                    if (hasFirstRoom)
                    {
                        r1 = new Room(roomRegex.Match(rawRoom).Groups[0].Value);
                        Common.DebugWriteLine(debug, "b1 = " + r1.Building + r1.Wing + r1.RoomNumber + r1.SubRoom);
                    }
                    else
                    {
                        r1 = new Room();
                    }
                    if (hasSecondRoom)
                    {
                        r2 = new Room(roomRegex.Match(rawRoom).Groups[5].Value);
                        Common.DebugWriteLine(debug, "b2 = " + r2.Building + r2.Wing + r2.RoomNumber + r2.SubRoom);
                    }
                    else
                    {
                        r2 = new Room();
                    }
                    //r1.RoomID = r2.RoomID = u1.UserID = u2.UserID = c.Section = 1;
                    //c.Section = 1;
                    string rawTime = ws.excelArray[currentRow, 4].Trim();
                    Schedule s = new Schedule(c, semester.SemesterID, u1.UserID, u2.UserID, r1.RoomID, r2.RoomID, rawTime);
                }
            }

            //Marshal.ReleaseComObject(ws);
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //            System.Environment.Exit(1);
        }

        static public void TestImportSchedule()
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


        static public void TestImportSemesters()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2015");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2016");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2017");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2018");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall 2019");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\spring 2016");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\spring 2017");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\spring 2018");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\spring 2019");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\summer 2016");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\summer 2017");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\summer 2018");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\summer 2019");
            TestSemester(@"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\winter 2016");

            watch.Stop();
            Console.WriteLine("Time elapsed as per stopwatch: {0} ", watch.Elapsed);
        }

        //todo If Fall, Spring or Winter semester
        //todo     Find date range for semester
        //todo  If Summer
        //todo    is it Session A B or C
        //todo    Find Date range.
        static public void TestSemester(string fileName)
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
            Common.DebugWriteLine(true, "");
            Common.DebugWriteLine(true, "GetExcelSchedule() fileName = " + fileName);
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
                Common.DebugWriteLine(true, "This is summer");
            }
            else
            {
                string semesterDateRange = FindString(semesterDateRangeSearchPath, ws, semesterDateRangeRegex);
                Common.DebugWriteLine(true, "semesterDateRangeRegex = " + semesterDateRange);
                isValidSemesterDateRange = semesterDateRange.Length > 0;
                if (isValidSemesterDateRange)
                {
                    calendar = new Calendar(semesterDateRange, semester.SemesterID, 1);
                }
                InsertCourses(ws, semester.SemesterID, ws.rowCount - 1);

            }

            //Marshal.ReleaseComObject(ws);
            //GC.Collect();rr
            //GC.WaitForPendingFinalizers();
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



        static void InsertCourses(ExcelData ws, int semesterId, int lastRow)
        {
            Common.DebugWriteLine(true, "semesterId = " + semesterId);

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

                    User u1 = GetUser(1, ws, currentRow);
                    User u2 = GetUser(2, ws, currentRow);

                    string rawRoom = ws.excelArray[currentRow, 5].Trim();
                    Room r1, r2;
                    string room1 = roomRegex.Match(rawRoom).Groups[1].Value;
                    string room2 = roomRegex.Match(rawRoom).Groups[6].Value;
                    bool hasFirstRoom = room1.Length != 0;
                    bool hasSecondRoom = room2.Length != 0;
                    if (hasFirstRoom)
                    {
                        r1 = new Room(roomRegex.Match(rawRoom).Groups[0].Value);
                        Common.DebugWriteLine(debug, "b1 = " + r1.Building + r1.Wing + r1.RoomNumber + r1.SubRoom);
                    }
                    else
                    {
                        r1 = new Room();
                    }
                    if (hasSecondRoom)
                    {
                        r2 = new Room(roomRegex.Match(rawRoom).Groups[5].Value);
                        Common.DebugWriteLine(debug, "b2 = " + r2.Building + r2.Wing + r2.RoomNumber + r2.SubRoom);
                    }
                    else
                    {
                        r2 = new Room();
                    }
                    string rawTime = ws.excelArray[currentRow, 4].Trim();
                    Schedule s = new Schedule(c, semesterId, u1.UserID, u2.UserID, r1.RoomID, r2.RoomID, rawTime);
                }
            }

        }

        static User GetUser(int userNumber, ExcelData ws, int row)
        {
            string rawUser = ws.excelArray[row, 3].Trim();
            string user = userRegex.Match(rawUser).Groups[userNumber].Value;
            bool hasUser = user.Length != 0;
            if (hasUser)
            {
                return new User(user);
            }
            return new User();
        }

        static User GetRoom(int roomNumber, ExcelData ws, int row)
        {
            string rawRoom = ws.excelArray[row, 3].Trim();
            string room = userRegex.Match(rawRoom).Groups[roomNumber].Value;
            bool hasRoom = room.Length != 0;
            if (hasRoom)
            {
                return new User(room);
            }
            return new User();
        }




    }




}
