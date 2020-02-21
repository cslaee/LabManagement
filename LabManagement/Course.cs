using System;
using System.Text.RegularExpressions;

namespace LabManagement
{
    internal class Course
    {
        public int CourseFK { get; set; }
        public string Subject { get; set; }
        public int Catalog { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Credit { get; set; }
        public int Laboratory { get; set; }
        public int PrerequisiteFK { get; set; }
        public int ClassCol { get; set; }
        public int Section { get; set; }  //Not stored in Db.  Only for excel import.
        const bool debug = Constants.courseDebug;

        public Course(string rawCourse, string title, string creditStr)
        {
            Regex coursePattern = new Regex(@"([A-Z]{1,4})\s?(\d{4})-?(\d{0,2})");
            Subject = coursePattern.Match(rawCourse).Groups[1].ToString();
            int.TryParse(coursePattern.Match(rawCourse).Groups[2].Value, out int catalog);
            int.TryParse(coursePattern.Match(rawCourse).Groups[3].Value, out int section);
            Catalog = catalog;
            Section = section;
            Title = title;
            int.TryParse(creditStr, out int credit);
            Credit = credit;
            Common.DebugWriteLine(debug, "Incoming Subject = " + Subject + ", Catalog = " + Catalog + ", Title = " + Title + " Credit = " + Credit);
            string[] colname = new[] { "subject", "catalog", "title", "credit" };
            var coldata = new object[] { Subject, Catalog, Title, Credit };

            var tuple = Db.GetTuple("Course", "*", colname, coldata);
            //bool coarseIsInDb = tuple.Count > 0;
            bool coarseNotInDb = tuple.Count == 0;

            if (coarseNotInDb)
            {
                CourseFK = Db.Insert("Course", colname, coldata);
                Common.DebugWriteLine(debug, "Course  " + Title + " with CourseFK = " + CourseFK + ", inserted into Db");
            }
            else
            {
                CourseFK = Convert.ToInt32(tuple[0].ToString());
                Common.DebugWriteLine(debug, "Course  " + Title + " with CourseFK = " + CourseFK + ", was in Db");
            }
        }


        //public Course(string lockNumber)
        public Course(int n)
        {
            string[] colname = new[] { "courseID" };
            var coldata = new object[] { n };
            var tuple = Db.GetTuple("Course", "*", colname, coldata);
            bool noLockInDb = tuple.Count == 0;


            if (noLockInDb)
            {
                Subject = "ERROR";
                return;
            }
            Subject = tuple[1].ToString(); //Working
            Catalog = Convert.ToInt32(tuple[2].ToString());// Not Working
            Title = tuple[3].ToString(); //Working
            Description = tuple[4].ToString();
            Credit =  Convert.ToInt32(tuple[5].ToString());
            Laboratory = 4;// Convert.ToInt32(tuple[6].ToString());
            ClassCol = 5;// Convert.ToInt32(tuple[7].ToString());
        }






    }





    






}
