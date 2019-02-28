﻿using System;
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
            Regex coursePattern = new Regex(@"([A-Z]{1,4})(\d{4})-?(\d{0,2})");
            Regex facultyPattern = new Regex(@"(\w+)\/?(\w+)?");
            Regex timePattern = new Regex(@"^(TBA|[MTWRFSU])([MTWRFSU]?)([MTWRFSU]?)([MTWRFSU]?)\s(\d{3,4})([AP]?M?)-(\d{3,4})([AP]?M?)");
            Regex roomPattern = new Regex(@"^(ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?([A-F]|LH)?(\d{1,4})([A-G])?");
            string fileName = @"C:\Users\moberme\Documents\ArletteTestSchedule.xlsx";
            ExcelData ws = new ExcelData(fileName, 1);

            Boolean isCourse;
            string rawCourse, subject, catalog, section;
            string title;
            string credit;
            string rawFaculty, faculty1, faculty2;
            string rawTime, day1, day2, day3, day4, startTime, startTimeAPM, endTime, endTimeAPM;
            string rawRoom, building, roomPrefix, roomPostfix, room;
            string outString;
            Semester semester = new Semester(ws);
            Console.WriteLine("name = " + semester.name);
            for (int currentRow = 5; currentRow <= ws.rowCount - 1; currentRow++)
            {
                rawCourse = ws.excelArray[currentRow, 0];
                isCourse = coursePattern.IsMatch(rawCourse);

                if (isCourse)
                {
                    subject = coursePattern.Match(rawCourse).Groups[1].Value;
                    catalog = coursePattern.Match(rawCourse).Groups[2].Value;
                    section = coursePattern.Match(rawCourse).Groups[3].Value;
                    title = ws.excelArray[currentRow, 1].Trim();
                    credit = ws.excelArray[currentRow, 2].Trim();
                    rawFaculty = ws.excelArray[currentRow, 3].Trim();
                    faculty1 = facultyPattern.Match(rawFaculty).Groups[1].Value;
                    faculty2 = facultyPattern.Match(rawFaculty).Groups[2].Value;
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
                    outString = "rawRoom = " + rawRoom + "building = " + building + " roomPrefix = " + roomPrefix + " room = " + room + " roomPostfix = " + roomPostfix;


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
