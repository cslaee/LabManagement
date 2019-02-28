using System;
using System.Text.RegularExpressions;

namespace LabManagement
{
    internal class Semester
    {
        public int semesterID { get; set; }
        public int version { get; set; }
        public long nameFK { get; set; }
        public int year { get; set; }
        public DateTime scheduleDate { get; set; }
        public DateTime schedulePostDate { get; set; }
        public string name { get; set; }
        public string scheduleDateStr { get; set; }
        public string schedulePostDateStr { get; set; }

        public Semester() { }

        public Semester(ExcelData ws)
        {
            Regex semesterPattern = new Regex(@"^(\d{1,2})\/(\d{1,2})\/(\d{4}).*?(FALL|WINTER|SPRING|SUMMER)\s(\d{4})");
            int m, d, y, semesterYearTemp;
            string rawSemester = ws.excelArray[2, 0].Trim();
            string revisionMonth = semesterPattern.Match(rawSemester).Groups[1].Value;
            string revisionDay = semesterPattern.Match(rawSemester).Groups[2].Value;
            string revisionYear = semesterPattern.Match(rawSemester).Groups[3].Value;
            name = semesterPattern.Match(rawSemester).Groups[4].Value ;
            string semesterYear = semesterPattern.Match(rawSemester).Groups[5].Value;

            int.TryParse(semesterYear, out semesterYearTemp);
            int.TryParse(revisionMonth, out m);
            int.TryParse(revisionDay, out d);
            int.TryParse(revisionYear, out y);
            year = semesterYearTemp;
            schedulePostDateStr = DateTime.Now.ToString("yyyy-M-d HH:mm:ss");
            scheduleDateStr = y + "-" + m + "-" + d;  
            string outString = "revisionMonth = " + m + " revisionDay = " + d + " revisionYear ='" + y + " semesterSeason = " + name + " semesterYear ='" + year + " update " + schedulePostDateStr;

            nameFK = Db.GetSingleInt("SemesterName", "name", "'" + name + "'", "semesterNameID");
            Console.WriteLine("getting text for id = " + nameFK);
            var thisSemesterAndYear = Db.GetTuple(this, "year = '" + year + "' AND nameFK = '" + nameFK + "'");

            if (thisSemesterAndYear.Count > 0)
            {
                semesterID = Convert.ToInt32(thisSemesterAndYear[0].ToString());
                version = Convert.ToInt32(thisSemesterAndYear[1].ToString()) + 1;
                string updateStr = "version = '" + version + "'" + ", scheduleDate = '" + scheduleDateStr + "', schedulePostDate = '" + schedulePostDateStr + "'";
                Db.UpdateID("Semester", "semesterID", semesterID, updateStr); 
            }
            else
            {
                Db.SqlInsert("Semester", "version, nameFK, year, scheduleDate, schedulePostDate", 1 + ", " + nameFK + "," + year + ",'" + scheduleDateStr  + "','" + schedulePostDateStr + "'");
            }
        }

    }
}
