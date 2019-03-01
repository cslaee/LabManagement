using System;
using System.Text.RegularExpressions;

namespace LabManagement
{
    internal class Course
    {
        public int CourseID { get; set; }
        public string Subject { get; set; }
        public int Catalog { get; set; }
        public string Title { get; set; }
        public int Description { get; set; }
        public int Credit { get; set; }
        public int Laboratory { get; set; }
        public int PrerequisiteFK { get; set; }
        public int ClassCol { get; set; }
        public int Section { get; set; }


        public Course(string rawCourse, string title, string creditStr)
        {
            Regex coursePattern = new Regex(@"([A-Z]{1,4})(\d{4})-?(\d{0,2})");
            Subject = coursePattern.Match(rawCourse).Groups[1].ToString();
            int.TryParse(coursePattern.Match(rawCourse).Groups[2].Value, out int catalog);
            int.TryParse(coursePattern.Match(rawCourse).Groups[3].Value, out int section);
            Catalog = catalog;
            Section = section;
            Title = title;
            int.TryParse(creditStr, out int credit);
            Credit = credit;
            var courseTuple = Db.GetTuple(this, "subject = '" + Subject + "' AND catalog = '" + Catalog + "' AND credit = '" + Credit +"'");

            if (courseTuple.Count > 0)
            {
                CourseID = Convert.ToInt32(courseTuple[0].ToString());
                string dbTitle = courseTuple[3].ToString();
                bool isdbTitleNotEqualToTitle = dbTitle != Title;
                if (isdbTitleNotEqualToTitle)
                {
                    string updateStr = "title = '" + Title + "'";
                    Console.WriteLine("Updated this coarse name from " + dbTitle + " to " + Title);
                    Db.UpdateID("Course", "courseID", CourseID, updateStr);
                }
            }
            else
            {
                string insertColumns = "subject, catalog, title, credit";
                string insertData = "'" + Subject + "', '" + Catalog + "', '" + Title + "', '" + Credit + "'";
                CourseID = Db.SqlInsert("Course", insertColumns, insertData); 
                Console.Write("Inserting Course" + insertColumns + " " + insertData + "Returned CourseId =" + CourseID);
            }
        }

        
    }
}
