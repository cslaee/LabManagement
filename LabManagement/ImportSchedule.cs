using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace LabManagement
{
    static class ImportSchedule
    {
        static readonly bool debug = Constants.importScheduleDebug;

        static public void GetExcelSchedule()
        {

            Web.BuildSchedule();
            System.Environment.Exit(1);


            Regex coursePattern = new Regex(@"([A-Z]{1,4})\s?(\d{4})-?(\d{0,2})");
            Regex userPattern = new Regex(@"(\w+)\/?(\w+)?");
            Regex roomPattern = new Regex(@"^(ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?([A-F]|LH)?(\d{1,4})([A-G])?\/?((ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?([A-F]|LH)?(\d{1,4})([A-G])?)?");
            string fileName = @"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall2019.xlsx";
            //string fileName = @"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\sum2019.xlsx";
            //string fileName = @"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\ArletteTestSchedule.xlsx";
            ExcelData ws = new ExcelData(fileName, 1);

            string rawSemester = ws.excelArray[2, 0].Trim();
            Semester semester = new Semester(rawSemester);

            string[] colname = new[] { "semesterFK" };
            var coldata = new object[] { semester.SemesterID };
            Db.Delete("Schedule", colname, coldata);

            for (int currentRow = 4; currentRow <= ws.rowCount - 1; currentRow++)
            {
                string rawCourse = ws.excelArray[currentRow, 0];
                bool isCourse = coursePattern.IsMatch(rawCourse);

                if (isCourse)
                {
                    string title = ws.excelArray[currentRow, 1].Trim();
                    string credit = ws.excelArray[currentRow, 2].Trim();
                    Course c = new Course(rawCourse, title, credit);

                    string coarseAndSection = c.Catalog + "-" + c.Section;
                    User u1, u2;
                    string rawUser = ws.excelArray[currentRow, 3].Trim();
                    string user1 = userPattern.Match(rawUser).Groups[1].Value;
                    string user2 = userPattern.Match(rawUser).Groups[2].Value;
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
                    string room1 = roomPattern.Match(rawRoom).Groups[1].Value;
                    string room2 = roomPattern.Match(rawRoom).Groups[6].Value;
                    bool hasFirstRoom = room1.Length != 0;
                    bool hasSecondRoom = room2.Length != 0;
                    if (hasFirstRoom)
                    {
                        r1 = new Room(roomPattern.Match(rawRoom).Groups[0].Value);
                        Common.DebugMessageCR(debug, "b1 = " + r1.Building + r1.Wing + r1.RoomNumber + r1.SubRoom);
                    }
                    else
                    {
                        r1 = new Room();
                    }
                    if (hasSecondRoom)
                    {
                        r2 = new Room(roomPattern.Match(rawRoom).Groups[5].Value);
                        Common.DebugMessageCR(debug, "b2 = " + r2.Building + r2.Wing + r2.RoomNumber + r2.SubRoom);
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
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
