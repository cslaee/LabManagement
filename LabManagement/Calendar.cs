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
        public int Subject { get; set; }
        public int SemesterFK { get; set; }
        public int EventTypeFK { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        static readonly bool debug = Constants.calendarDebug;

        public Calendar() { }

        public Calendar(string dateRange, int semesterFK, int eventTypeFK)
        {
            Regex semesterDateRangeRegex = new Regex(Constants.semesterDateRangePattern);
            SemesterFK = semesterFK;
            EventTypeFK = eventTypeFK;
            string monthStartStr = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(semesterDateRangeRegex.Match(dateRange).Groups[1].Value.ToLower());
            string dayStart = semesterDateRangeRegex.Match(dateRange).Groups[2].Value;
            string yearStart = semesterDateRangeRegex.Match(dateRange).Groups[3].Value;
            string monthEndStr = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(semesterDateRangeRegex.Match(dateRange).Groups[5].Value.ToLower());
            string dayEnd = semesterDateRangeRegex.Match(dateRange).Groups[6].Value;
            string yearEnd = semesterDateRangeRegex.Match(dateRange).Groups[7].Value;
            int  monthStart = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(monthStartStr) + 1;
            int  monthEnd = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(monthEndStr.ToLower()) + 1;
            string output = "mss=" + monthStartStr + " ms=" + monthStart + " ds=" + dayStart  + " ys=" + yearStart + "mes=" + monthEndStr + " me=" + monthEnd + " de=" + dayEnd + " ye=" + yearEnd;
            Common.DebugWriteLine(debug, output);
            //Name = semesterNameAndYearRegex .Match(rawSemester).Groups[1].Value;
            //string semesterYear = semesterNameAndYearRegex .Match(rawSemester).Groups[2].Value;

            //int.TryParse(semesterYear, out int semesterYearTemp);
            //int.TryParse(revisionMonth, out int m);
            //int.TryParse(revisionDay, out int d);
            //int.TryParse(revisionYear, out int y);
            //Year = semesterYearTemp;
            //SchedulePostDateStr = DateTime.Now.ToString("yyyy-M-d HH:mm:ss");
            //ScheduleDateStr = y + "-" + m + "-" + d;










            Common.DebugWriteLine(debug, "Creating a new Calendar");
        }
    }
}
