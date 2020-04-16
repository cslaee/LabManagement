using System;

namespace LabManagement
{
    internal class Calendar
    {
        public int CalendarID { get; set; }
        public string Subject { get; set; }
        public int SemesterFK { get; set; }
        public int EventTypeFK { get; set; }
        public string EventTypeDescription { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateStr { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDateStr { get; set; }
        const bool debug = Constants.calendarDebug;

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


        public Calendar(string startDate, string endDate, Semester semester, string eventTypeStr)
        {
            SemesterFK = semester.SemesterID;
            EventTypeDescription = eventTypeStr;
//            EventTypeFK = 1;





            string[] colname = new[] { "description", "eventTypeID" };
            var coldata = new object[] { EventTypeDescription, EventTypeFK}; 
            var tuple = Db.GetTuple("EventType", "*", colname, coldata);
            bool coarseNotInDb = tuple.Count == 0;

            if (coarseNotInDb)
            {
                EventTypeFK = Db.Insert("EventType", colname, coldata);
                Common.DebugWriteLine(debug, "EventTypeDescription  " + EventTypeDescription + " with EventTypeFK = " + EventTypeFK + ", inserted into Db");
            }
            else
            {
                EventTypeFK = Convert.ToInt32(tuple[0].ToString());
                Common.DebugWriteLine(debug, "EventTypeDescription " + EventTypeDescription + " with EventTypeFK = " + EventTypeFK + ", was in Db");
            }














            string DefaultEventType = "Semester Start and Stop Date";

            StartDateStr = startDate; ;
            EndDateStr = endDate;

            Common.DebugWriteLine(debug, StartDateStr);
            Common.DebugWriteLine(debug, EndDateStr);

            colname = new[] { "semesterFK", "eventTypeFK" };
            coldata = new object[] { SemesterFK, "1" };
            Db.Delete("Calendar", colname, coldata);

            colname = new[] { "semesterNameID" };
            coldata = new object[] { semester.NameFK };
            tuple = Db.GetTuple("SemesterName", "name", colname, coldata);
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
