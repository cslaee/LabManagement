using System;
using System.Text.RegularExpressions;

namespace LabManagement
{
    internal class Semester
    {
        public int SemesterID { get; set; }
        public int Version { get; set; }
        public long NameFK { get; set; }
        public int Year { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime SchedulePostDate { get; set; }
        public string Name { get; set; }
        public string ScheduleDateStr { get; set; }
        public string SchedulePostDateStr { get; set; }
        static readonly bool debug = Constants.semesterDebug;

        public Semester() { }

        //public Semester(string rawRevisionDate, string rawSemester)
        public Semester(string rawRevisionDate, string name, string semesterYear, long nameFK)
        {
            Regex revisionDateRegex = new Regex(Constants.revisionDatePattern);
            Regex semesterNameAndYearRegex = new Regex(Constants.semesterNameAndYearPattern);
            string revisionMonth = revisionDateRegex.Match(rawRevisionDate).Groups[1].Value;
            string revisionDay = revisionDateRegex.Match(rawRevisionDate).Groups[2].Value;
            string revisionYear = revisionDateRegex.Match(rawRevisionDate).Groups[3].Value;
            Name = name; 
            // Name = semesterNameAndYearRegex.Match(rawSemester).Groups[1].Value;
            // string semesterYear = semesterNameAndYearRegex .Match(rawSemester).Groups[2].Value;

            int.TryParse(semesterYear, out int semesterYearTemp);
            int.TryParse(revisionMonth, out int m);
            int.TryParse(revisionDay, out int d);
            int.TryParse(revisionYear, out int y);
            Year = semesterYearTemp;
            SchedulePostDateStr = DateTime.Now.ToString("yyyy-M-d HH:mm:ss");
            ScheduleDateStr = y + "-" + m + "-" + d;

            //NameFK = Db.GetTupleInt("SemesterName", "semesterNameID", "name", Name);
            NameFK = nameFK;
            string[] colnameLookup = new[] { "year", "nameFK" };
            var coldataLookup = new object[] { Year, NameFK }; 
            var tuple = Db.GetTuple("Semester", "*", colnameLookup, coldataLookup);
            bool hasSemesterInDb = tuple.Count > 0;

            if (hasSemesterInDb)
            {
                SemesterID = Convert.ToInt32(tuple[0].ToString());
                Version = Convert.ToInt32(tuple[1].ToString()) + 1;
                string updateStr = "version = '" + Version + "'" + ", scheduleDate = '" + ScheduleDateStr + "', schedulePostDate = '" + SchedulePostDateStr + "'";
                Db.Update("Semester", "semesterID", SemesterID, updateStr);
                Common.DebugWriteLine(debug, "Updating Semester " + SemesterID + " " + updateStr);
            }
            else
            {
                string[] colname = new[] { "version", "nameFK", "year", "scheduleDate", "schedulePostDate" };
                var coldata = new object[] { 1, NameFK, Year, ScheduleDateStr, SchedulePostDateStr };
                SemesterID = Db.Insert("Semester", colname, coldata);
                Common.DebugWriteLine(debug, "Inserting Semester" + colname + " " + coldata + "ReturnedId =" + SemesterID);
            }
        }

        public Semester(string rawSemester)
        {
            Regex semesterPattern = new Regex(@"^(\d{1,2})\/(\d{1,2})\/(\d{4}).*?(FALL|WINTER|SPRING|SUMMER)\s(\d{4})");
            string revisionMonth = semesterPattern.Match(rawSemester).Groups[1].Value;
            string revisionDay = semesterPattern.Match(rawSemester).Groups[2].Value;
            string revisionYear = semesterPattern.Match(rawSemester).Groups[3].Value;
            Name = semesterPattern.Match(rawSemester).Groups[4].Value;
            string semesterYear = semesterPattern.Match(rawSemester).Groups[5].Value;

            int.TryParse(semesterYear, out int semesterYearTemp);
            int.TryParse(revisionMonth, out int m);
            int.TryParse(revisionDay, out int d);
            int.TryParse(revisionYear, out int y);
            Year = semesterYearTemp;
            SchedulePostDateStr = DateTime.Now.ToString("yyyy-M-d HH:mm:ss");
            ScheduleDateStr = y + "-" + m + "-" + d;

            NameFK = Db.GetTupleInt("SemesterName", "semesterNameID", "name", Name);
            string[] colnameLookup = new[] { "year", "nameFK" };
            var coldataLookup = new object[] { Year, NameFK }; 
            var tuple = Db.GetTuple("Semester", "*", colnameLookup, coldataLookup);
            bool hasSemesterInDb = tuple.Count > 0;

            if (hasSemesterInDb)
            {
                SemesterID = Convert.ToInt32(tuple[0].ToString());
                Version = Convert.ToInt32(tuple[1].ToString()) + 1;
                string updateStr = "version = '" + Version + "'" + ", scheduleDate = '" + ScheduleDateStr + "', schedulePostDate = '" + SchedulePostDateStr + "'";
                Db.Update("Semester", "semesterID", SemesterID, updateStr);
                Common.DebugWriteLine(debug, "Updating Semester " + SemesterID + " " + updateStr);
            }
            else
            {
                string[] colname = new[] { "version", "nameFK", "year", "scheduleDate", "schedulePostDate" };
                var coldata = new object[] { 1, NameFK, Year, ScheduleDateStr, SchedulePostDateStr };
                SemesterID = Db.Insert("Semester", colname, coldata);
                Common.DebugWriteLine(debug, "Inserting Semester" + colname + " " + coldata + "ReturnedId =" + SemesterID);
            }
        }





    }
}
