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
        //*todo get Semester name & year
        //*toto get semester dates
        //*todo get revision date

        //todo Insert Semester season, year, scheduleDate, schedulePostDate

        //todo Insert Course | Subject, catalog, Title, Credit
        //todo Insert User
        //todo Insert Schedule | lookup ClassID, Insert ClassID, Section, SemesesterID, Days, StartTime, EndTime, InstructorID, RoomID
        //todo Does Class DefaultRoom match Schedule Room?
        //todo Does Class maxSections match Schedule Section?
        //todo Does default room = scheduled room?

        static public void GetExcelSchedule()
        {
            Regex coursePattern = new Regex(@"([A-Z]{1,4})\s?(\d{4})-?(\d{0,2})");
            //Regex coursePattern = new Regex(@"([A-Z]{1,4})\s?(\d{4})-?(\d{0,2})");
            Regex facultyPattern = new Regex(@"(\w+)\/?(\w+)?");
            Regex timePattern = new Regex(@"^(TBA|[MTWRFSU])([MTWRFSU]?)([MTWRFSU]?)([MTWRFSU]?)\s(\d{3,4})([AP]?M?)-(\d{3,4})([AP]?M?)");
            Regex roomPattern = new Regex(@"^(ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?([A-F]|LH)?(\d{1,4})([A-G])?");
            string fileName = @"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\fall2018.xlsx";
            //string fileName = @"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\sum2019.xlsx";
            //string fileName = @"C:\Users\moberme\Documents\LabManagement\ArletteSchedules\ArletteTestSchedule.xlsx";
            ExcelData ws = new ExcelData(fileName, 1);

            Boolean isCourse;
            string rawCourse, title, credit;
            string rawFaculty, faculty1, faculty2;
            string rawTime, day1, day2, day3, day4, startTime, startTimeAPM, endTime, endTimeAPM;
            string rawRoom, building, roomPrefix, roomPostfix, room;
            string outString;
            //            Semester semester = new Semester(ws);
            //            Console.WriteLine("name = " + semester.Name);
            for (int currentRow = 4; currentRow <= ws.rowCount - 1; currentRow++)
            {
                rawCourse = ws.excelArray[currentRow, 0];
                isCourse = coursePattern.IsMatch(rawCourse);

                //System.Console.WriteLine(" 0 = " + ws.excelArray[currentRow, 0] + " 1 = " + ws.excelArray[currentRow, 1] + " 2 = " + ws.excelArray[currentRow, 2] + " 3 = " + ws.excelArray[currentRow, 3]);
                if (isCourse)
                {
                    title = ws.excelArray[currentRow, 1].Trim();
                    credit = ws.excelArray[currentRow, 2].Trim();
                    Course c = new Course(rawCourse, title, credit);

                    string coarseAndSection = c.Catalog + "-" + c.Section;
                    User u1, u2;
                    rawFaculty = ws.excelArray[currentRow, 3].Trim();
                    faculty1 = facultyPattern.Match(rawFaculty).Groups[1].Value;
                    faculty2 = facultyPattern.Match(rawFaculty).Groups[2].Value;
                    bool hasFirstUser = faculty1.Length != 0;
                    bool hasSecondUser = faculty2.Length != 0;
                    if (hasFirstUser)
                    {
                        u1 = new User(faculty1);
                    }
                    else
                    {
                        u1 = new User("TBA");
                    }
                    if (hasSecondUser)
                    {
                        u2 = new User(faculty2);
                        //Console.WriteLine(coarseAndSection + " u2 = " + u2.Last);
                    }

                    rawTime = ws.excelArray[currentRow, 4].Trim();
                    day1 = timePattern.Match(rawTime).Groups[1].Value;
                    day2 = timePattern.Match(rawTime).Groups[2].Value;
                    day3 = timePattern.Match(rawTime).Groups[3].Value;
                    day4 = timePattern.Match(rawTime).Groups[4].Value;
                    startTime = timePattern.Match(rawTime).Groups[5].Value;
                    startTimeAPM = timePattern.Match(rawTime).Groups[6].Value;
                    endTime = timePattern.Match(rawTime).Groups[7].Value;
                    endTimeAPM = timePattern.Match(rawTime).Groups[8].Value;

                    rawRoom = ws.excelArray[currentRow, 5].Trim();
                    building = roomPattern.Match(rawRoom).Groups[1].Value;
                    roomPrefix = roomPattern.Match(rawRoom).Groups[2].Value;
                    room = roomPattern.Match(rawRoom).Groups[3].Value;
                    roomPostfix = roomPattern.Match(rawRoom).Groups[4].Value;

                    //string outString = courseNumber + " " + section + " " + title + " " + credit + " " + faculty + " " + days + " " + startTime + " " + endTime + " " + room;
                    //string outString = "course letters = " + courseLetters+ " number = " + courseNumber + " section ='" + section + "'";
                    //string outString = "faculty 1 = " + faculty1 + " faculty 2 = " + "'" + faculty2 + "'";
                    //string outString = "rawCourse = " + rawCourse + " day 1 = " + day1 + " day 2 = " + day2+ " day 3 = " + day3+ " day 4 = " + day4 + " startTime = " + startTime + " startTimeAPM = " + startTimeAPM + " endTime = " + endTime + " endTimeAPM = " + endTimeAPM;
                    //outString = "rawRoom = " + rawRoom + "building = " + building + " roomPrefix = " + roomPrefix + " room = " + room + " roomPostfix = " + roomPostfix;
                    outString = "subject =" + c.Subject + " catalog =" + c.Catalog + " section =" + c.Section + " title=" + c.Title + " credit =" + c.Credit;

                    //                 var match = Regex.Match(ws.excelArray[currentRow, 0], pattern, RegexOptions.IgnoreCase);

                    //courseNumber = course_Number.Match(ws.excelArray[currentRow, 0], "").Value;
                    //Regex.Replace(ws.excelArray[currentRow, 0], pattern, String.Empty)
                    //   Console.WriteLine(outString);
                }
                //System.Console.WriteLine("in loop ");
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
