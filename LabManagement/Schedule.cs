using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LabManagement
{
    class Schedule
    {
        public int ScheduleID { set; get; }
        public int CourseFK { set; get; }
        public int Section { set; get; }
        public int SemesterFK { set; get; }
        public int Instructor1FK { set; get; }
        public int Instructor2FK { set; get; }
        public int Room1FK { set; get; }
        public int Room2FK { set; get; }
        public int StatusFK { set; get; }
        public int Days { set; get; }
        public DateTime StartTime { set; get; }
        public DateTime EndTime { set; get; }

        public Schedule(Course c, int semesterFK, int instructor1FK, int instructor2FK, int room1FK, int room2FK, string rawTime)
        {
            Regex timePattern = new Regex(@"^(TBA|[MTWRFSU])([MTWRFSU]?)([MTWRFSU]?)([MTWRFSU]?)\s(\d{1,4}):?(\d{2})?([aApP]?[mM]?)-(\d{1,4}):?(\d{2})?([aApP]?[mM]?)");
            CourseFK = c.CourseFK;
            Section = c.Section;
            SemesterFK = semesterFK;
            Instructor1FK = instructor1FK;
            Instructor2FK = instructor2FK;
            Room1FK = room1FK;
            Room2FK = room2FK;
            StatusFK = 1;
            Days = SetDaysOfWeek(rawTime);
            //TestDaysOfWeek(rawTime);
            string startTimeHours = timePattern.Match(rawTime).Groups[5].Value;
            string startTimeMinutes = timePattern.Match(rawTime).Groups[6].Value;
            string startTimeAPM = timePattern.Match(rawTime).Groups[7].Value;
            string endTimeHours = timePattern.Match(rawTime).Groups[8].Value;
            string endTimeMinutes = timePattern.Match(rawTime).Groups[9].Value;
            string endTimeAPM = timePattern.Match(rawTime).Groups[10].Value.ToLower();

            int hoursLength = endTimeHours.Length;
            bool isMinutesEmpty = endTimeMinutes.Length == 0;
            bool isMinutesPartOfHours = hoursLength > 2;
            if (isMinutesPartOfHours)
            {
                endTimeMinutes = endTimeHours.Substring(hoursLength - 2);
                endTimeHours = endTimeHours.Substring(0, hoursLength - 2);
            }
            else if (isMinutesEmpty)
            {
                endTimeMinutes = "00";
            }
            bool isEndTimePM = endTimeAPM.Equals("pm");

            int.TryParse(endTimeHours, out int endTimeHoursInt);
            bool isTimeHoursNot12 = endTimeHoursInt != 12;
            bool isPm = isEndTimePM & isTimeHoursNot12;
            endTimeHoursInt = ConvertToMilitaryTime(isPm, endTimeHoursInt);
            endTimeHours = endTimeHoursInt.ToString();

            hoursLength = startTimeHours.Length;
            isMinutesEmpty = startTimeMinutes.Length == 0;
            isMinutesPartOfHours = hoursLength > 2;
            if (isMinutesPartOfHours)
            {
                startTimeMinutes = startTimeHours.Substring(hoursLength - 2);
                startTimeHours = startTimeHours.Substring(0, hoursLength - 2);
            }
            else if (isMinutesEmpty)
            {
                startTimeMinutes = "00";
            }

            int.TryParse(startTimeHours, out int startTimeHoursInt);
            isPm = endTimeHoursInt - startTimeHoursInt > 10;
            startTimeHoursInt = ConvertToMilitaryTime(isPm, startTimeHoursInt);
            startTimeHours = startTimeHoursInt.ToString();

            Console.WriteLine(startTimeHours + ":" + startTimeMinutes + " " + endTimeHours + ":" + endTimeMinutes);


            string[] colname = new[] { "classFK", "section", "semesterFK", "days", "startTime", "endTime", "statusFK", "roomFK" };
            //var coldata = new object[] { c.CourseFK, c.Section, semester.SemesterID, };
            //var tuple = Db.GetTuple("Schedule", "*", colname, coldata);

            //bool noCourseInDb = tuple.Count == 0;
            //if (noCourseInDb)
            //{
            //    Db.SqlInsert("Schedule", colname, coldata);
            //}


        }


        static int ConvertToMilitaryTime(bool isPm, int hours)
        {
            if (isPm)
            {
                hours = hours + 12;
            }
            return hours;
        }

        static int SetDaysOfWeek(string rawTime)
        {
            Regex timePattern = new Regex(@"^(TBA|[MTWRFSU])([MTWRFSU]?)([MTWRFSU]?)([MTWRFSU]?)\s(\d{1,4}):?(\d{2})?([aApP]?[mM]?)-(\d{1,4}):?(\d{2})?([aApP]?[mM]?)");
            int daysOfWeekBits = 0;
            for (int i = 1; i < 5; i++)
            {
                string currentDay = timePattern.Match(rawTime).Groups[i].Value.Trim().ToLower();
                switch (currentDay)
                {
                    case "s":
                        daysOfWeekBits = daysOfWeekBits + 1;
                        break;
                    case "m":
                        daysOfWeekBits = daysOfWeekBits + 2;
                        break;
                    case "t":
                        daysOfWeekBits = daysOfWeekBits + 4;
                        break;
                    case "w":
                        daysOfWeekBits = daysOfWeekBits + 8;
                        break;
                    case "r":
                        daysOfWeekBits = daysOfWeekBits + 16;
                        break;
                    case "f":
                        daysOfWeekBits = daysOfWeekBits + 32;
                        break;
                    case "u":
                        daysOfWeekBits = daysOfWeekBits + 64;
                        break;
                    default:
                        break;
                }
            }
            return daysOfWeekBits;
        }


        static string GetDaysOfWeek(int daysOfWeekBits)
        {
            uint mask = 1;
            var daysString = new System.Text.StringBuilder();
            for (int i = 1; i < 8; i++)
            {
                switch (daysOfWeekBits & mask)
                {
                    case 1:
                        daysString.Append("U");
                        break;
                    case 2:
                        daysString.Append("M");
                        break;
                    case 4:
                        daysString.Append("T");
                        break;
                    case 8:
                        daysString.Append("W");
                        break;
                    case 16:
                        daysString.Append("R");
                        break;
                    case 32:
                        daysString.Append("F");
                        break;
                    case 64:
                        daysString.Append("S");
                        break;
                }
                mask = mask << 1;
            }
            return daysString.ToString();
        }


        static void TestDaysOfWeek(string rawTime)
        {
            Regex timePattern = new Regex(@"^(TBA|[MTWRFSU])([MTWRFSU]?)([MTWRFSU]?)([MTWRFSU]?)\s(\d{1,4}):?(\d{2})?([aApP]?[mM]?)-(\d{1,4}):?(\d{2})?([aApP]?[mM]?)");
            string dow = timePattern.Match(rawTime).Groups[1].Value + timePattern.Match(rawTime).Groups[2].Value + timePattern.Match(rawTime).Groups[3].Value + timePattern.Match(rawTime).Groups[4].Value;
            int daysInt = SetDaysOfWeek(rawTime);
            string daysString = GetDaysOfWeek(daysInt);
            if (daysString.Equals(dow))
            {
                Console.WriteLine("Pass " + dow + "=" + daysString);
            }
            else
            {
                Console.WriteLine("Fail " + dow + "!=" + daysString);
            }
        }
    }
}
