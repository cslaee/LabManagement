using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public Calendar(string dateRange, Semester semester, int eventTypeFK)
        {
            Regex semesterDateRangeRegex = new Regex(Constants.semesterDateRangePattern);
            SemesterFK = semester.SemesterID;
            EventTypeFK = eventTypeFK;
            StartDateStr = GetDateString(semesterDateRangeRegex, dateRange, 1);
            EndDateStr = GetDateString(semesterDateRangeRegex, dateRange, 5);
            Common.DebugWriteLine(debug, StartDateStr);
            Common.DebugWriteLine(debug, EndDateStr);

            string[] colname = new[] { "semesterFK", "eventTypeFK"};
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
 
            colname = new[] { "semesterFK", "eventTypeFK", "subject", "startDate", "endDate"};
            coldata = new object[] { SemesterFK, EventTypeFK, Subject, StartDateStr, EndDateStr };
            Db.Insert("Calendar", colname, coldata);
        }


        string GetDateString(Regex dateRegex, string dateRange, int dateIndex)
        {
            string monthStr = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dateRegex.Match(dateRange).Groups[dateIndex].Value.ToLower());
            string dayStr = dateRegex.Match(dateRange).Groups[dateIndex + 1].Value;
            string yearStr = dateRegex.Match(dateRange).Groups[dateIndex + 2].Value;

            bool hasMonth = monthStr.Length > 2;
            if (hasMonth)
            {
                string monthShortStr = monthStr.Substring(0, 3);
                int monthInt = DateTime.ParseExact(monthShortStr, "MMM", CultureInfo.InvariantCulture).Month;
                return yearStr + "-" + monthInt + "-" + dayStr;
            }
            return "";
        }


    }
}
