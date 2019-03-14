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
        public int Description { get; set; }
        public int Credit { get; set; }
        public int Laboratory { get; set; }
        public int PrerequisiteFK { get; set; }
        public int ClassCol { get; set; }
        public int Section { get; set; }  //Not stored in Db.  Only for excel import.
        static readonly bool debug = Constants.courseDebug;

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
            string[] colname = new[] { "subject", "catalog", "title", "credit" };
            var coldata = new object[] { Subject, Catalog, Title, Credit };
            string[] colnameLookup = new[] { "subject", "catalog", "credit" };
            var coldataLookup = new object[] { Subject, Catalog, Credit };

            var tuple = Db.GetTuple("Course", "*", colnameLookup, coldataLookup);
            bool coarseIsInDb = tuple.Count > 0;

            if (coarseIsInDb)
            {
                CourseFK = Convert.ToInt32(tuple[0].ToString());
                string dbTitle = tuple[3].ToString();
                bool isTitleUnique = dbTitle != Title;

                if (isTitleUnique)
                {
                    bool isSpecialTopic = Catalog == 4540;
                    if (isSpecialTopic)
                    {
                        Db.SqlInsert("Course", colname, coldata);
                        Common.DebugMessageCR(debug, "Inserting Course" + colname + " " + coldata + "Returned CourseId =" + CourseFK);
                    }
                    else
                    {
                        string updateStr = "title = '" + Title + "'";
                        Common.DebugMessageCR(debug, "Updated this coarse name from " + dbTitle + " to " + Title);
                        Db.UpdateID("Course", "courseID", CourseFK, updateStr);
                    }
                }
            }
            else
            {
                Db.SqlInsert("Course", colname, coldata);
                //Console.Write("Inserting Course" + insertColumns + " " + insertData + "Returned CourseId =" + CourseID);
            }
        }

    }
}
