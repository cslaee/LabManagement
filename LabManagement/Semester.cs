using System;
using System.Collections.Generic;
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
        public string NameYear { get; set; }
        public string ScheduleDateStr { get; set; }
        public string SchedulePostDateStr { get; set; }
        const bool debug = Constants.semesterDebug;

        public Semester() { }

        //public Semester(string rawRevisionDate, string rawSemester)
        public Semester(string rawRevisionDate, string name, string semesterYear, long nameFK)
        {
            Regex revisionDateRegex = new Regex(Constants.revisionDatePattern);
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

        static public List<Semester> GetSemesterList()
        {
            List<Semester> semesterList = new List<Semester>();
            string startTimeStr; 
            string endTimeStr;
            
            string semesterNamesSQL = "SELECT  DISTINCT substr(name, 1, 3) ||  substr(year, 3, 4), semesterID, version, nameFK, year," +
                "scheduleDate, schedulePostDate, name FROM Semester " +
                "INNER JOIN SemesterName ON SemesterName.semesterNameID = Semester.nameFK ORDER BY year DESC, nameFK DESC";

            List <object> tuple = Db.GetTupleNewOne(semesterNamesSQL);
            int rowCount = tuple.Count / 8;

            for (int i = 0; i < rowCount; i++)
            {
                Semester s = new Semester
                {
                    NameYear = tuple[i * 8].ToString(),
                    SemesterID = Convert.ToInt32(tuple[i * 8 + 1].ToString()),
                    Version = Convert.ToInt32(tuple[i * 8 + 2].ToString()),
                    NameFK= Convert.ToInt32(tuple[i * 8 + 3].ToString()),
                    Year = Convert.ToInt32(tuple[i * 8 + 4].ToString()),
                    //ScheduleDate = Convert.ToDateTime(tuple[i * 8 + 5].ToString()),
                    ScheduleDateStr = "11/12/2010",//tuple[i * 8 + 5].ToString(),
                    SchedulePostDateStr = "11/12/2010 Semester.cs",//tuple[i * 8 + 6].ToString(),
                    Name = tuple[i * 8 + 7].ToString()
                };
                /*                startTimeStr = String.Format("{0:hmm}",heduleDate);

                                    StartTime = Convert.ToDateTime(tuple[i * 12 + 10].ToString()),
                                    EndTime = Convert.ToDateTime(tuple[i * 12 + 11].ToString())


                                endTimeStr = String.Format("{0:hmm}",s.EndTime);
                                s.DaysString = GetDaysOfWeek(s.Days) + " " + startTimeStr + "-" +endTimeStr;*/
                Common.DebugWriteLine(true, "test date = " + s.ScheduleDateStr );
                Common.DebugWriteLine(true, "test post date = " + s.SchedulePostDateStr  );
                semesterList.Add(s);
                
            }
 

            return semesterList;
        }



    }
}
