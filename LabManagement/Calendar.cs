using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LabManagement
{
    internal class Calendar
    {
        public int CalendarID { get; set; }
        public string Subject { get; set; }
        public int SemesterFK { get; set; }
        public int EventTypeFK { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateStr { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDateStr { get; set; }
        static readonly bool debug = Constants.calendarDebug;

        public Calendar() { }



        public Calendar(string startDate, string endDate, Semester semester)
        {
            SemesterFK = semester.SemesterID;
            EventTypeFK = 1;

            StartDateStr = startDate; ;
            EndDateStr = endDate;

            Common.DebugWriteLine(debug, StartDateStr);
            Common.DebugWriteLine(debug, EndDateStr);

            string[] colname = new[] { "semesterFK", "eventTypeFK" };
            var coldata = new object[] { SemesterFK, "1" };
            Db.Delete("Calendar", colname, coldata);

            colname = new[] { "semesterNameID" };
            coldata = new object[] { semester.NameFK };
            var tuple = Db.GetTuple("SemesterName", "name", colname, coldata);
            bool hasSemesterName = tuple.Count > 0;
            if (hasSemesterName)
            {
                Subject = tuple[0].ToString() + " " + semester.Year;
            }

            colname = new[] { "semesterFK", "eventTypeFK", "subject", "startDate", "endDate" };
            coldata = new object[] { SemesterFK, EventTypeFK, Subject, StartDateStr, EndDateStr };
            Db.Insert("Calendar", colname, coldata);
        }














        public Calendar(string dateRange, Semester semester, int eventTypeFK)
        {
            Regex semesterDateRangeRegex = new Regex(Constants.semesterDateRangePattern);
            SemesterFK = semester.SemesterID;
            EventTypeFK = eventTypeFK;
            //StartDateStr = GetDateString(semesterDateRangeRegex, dateRange, 1);
            //EndDateStr = GetDateString(semesterDateRangeRegex, dateRange, 5);

            string yearStr = semester.Year.ToString();

            var monthDay = Common.GetStartAndStopMonthDay(semesterDateRangeRegex, dateRange, 1, 5);
            StartDateStr = yearStr + monthDay[0].ToString(); 
            EndDateStr = yearStr + monthDay[1].ToString(); 




            //StartDateStr = yearStr + GetMonthDayString(semesterDateRangeRegex, dateRange, 1);
            //EndDateStr = yearStr + GetMonthDayString(semesterDateRangeRegex, dateRange, 5);

            Common.DebugWriteLine(debug, StartDateStr);
            Common.DebugWriteLine(debug, EndDateStr);

            string[] colname = new[] { "semesterFK", "eventTypeFK" };
            var coldata = new object[] { SemesterFK, "1" };
            Db.Delete("Calendar", colname, coldata);

            colname = new[] { "semesterNameID" };
            coldata = new object[] { semester.NameFK };
            var tuple = Db.GetTuple("SemesterName", "name", colname, coldata);
            bool hasSemesterName = tuple.Count > 0;
            if (hasSemesterName)
            {
                Subject = tuple[0].ToString() + " " + semester.Year;
            }

            colname = new[] { "semesterFK", "eventTypeFK", "subject", "startDate", "endDate" };
            coldata = new object[] { SemesterFK, EventTypeFK, Subject, StartDateStr, EndDateStr };
            Db.Insert("Calendar", colname, coldata);
        }


        public Calendar(string dateRange, Semester semester)
        {
            Regex summerSessionABCRegex = new Regex(Constants.summerSessionABCPattern);
            SemesterFK = semester.SemesterID;
            EventTypeFK = 1;
            string yearStr = semester.Year.ToString();

            var monthDay = Common.GetStartAndStopMonthDay(summerSessionABCRegex, dateRange, 4, 7);
            StartDateStr = yearStr + monthDay[0].ToString(); 
            EndDateStr = yearStr + monthDay[1].ToString(); 


            //StartDateStr = yearStr + GetMonthDayString(summerSessionABCRegex, dateRange, 4);
            //EndDateStr = yearStr + GetMonthDayString(summerSessionABCRegex, dateRange, 7);

            Common.DebugWriteLine(debug, StartDateStr);
            Common.DebugWriteLine(debug, EndDateStr);

            string[] colname = new[] { "semesterFK", "eventTypeFK" };
            var coldata = new object[] { SemesterFK, "1" };
            Db.Delete("Calendar", colname, coldata);

            colname = new[] { "semesterNameID" };
            coldata = new object[] { semester.NameFK };
            var tuple = Db.GetTuple("SemesterName", "name", colname, coldata);
            bool hasSemesterName = tuple.Count > 0;
            if (hasSemesterName)
            {
                Subject = tuple[0].ToString() + " " + semester.Year;
            }

            colname = new[] { "semesterFK", "eventTypeFK", "subject", "startDate", "endDate" };
            coldata = new object[] { SemesterFK, EventTypeFK, Subject, StartDateStr, EndDateStr };
            Db.Insert("Calendar", colname, coldata);
        }

    }
}
