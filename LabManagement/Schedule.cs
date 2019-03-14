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
            Regex timePattern = new Regex(@"^(TBA|[MTWRFSU])([MTWRFSU]?)([MTWRFSU]?)([MTWRFSU]?)\s(\d{1,4})([AP]?M?)-(\d{1,4})([AP]?M?)");
            CourseFK = c.CourseFK;
            Section = c.Section;
            SemesterFK = semesterFK;
            Instructor1FK = instructor1FK;
            Instructor2FK = instructor2FK;
            Room1FK = room1FK;
            Room2FK = room2FK;
            StatusFK = 1;
            Days = SetDaysOfWeek(rawTime);
            //Console.WriteLine(Days);
            TestDaysOfWeek(rawTime);
            //GetDaysOfWeek(Days);
            //string day1 = timePattern.Match(rawTime).Groups[1].Value;
            //Console.Write(timePattern.Match(rawTime).Groups[1].Value +"1="+SetDaysOfWeek(timePattern.Match(rawTime).Groups[1].Value) + ", ");
            //Console.Write(timePattern.Match(rawTime).Groups[2].Value +"2="+SetDaysOfWeek(timePattern.Match(rawTime).Groups[2].Value) + ", ");
            //Console.Write(timePattern.Match(rawTime).Groups[3].Value +"3="+SetDaysOfWeek(timePattern.Match(rawTime).Groups[3].Value) + ", ");
            //Console.WriteLine(timePattern.Match(rawTime).Groups[4].Value +"4="+SetDaysOfWeek(timePattern.Match(rawTime).Groups[4].Value) );
            string startTime = timePattern.Match(rawTime).Groups[5].Value;
            string startTimeAPM = timePattern.Match(rawTime).Groups[6].Value;
            string endTime = timePattern.Match(rawTime).Groups[7].Value;
            string endTimeAPM = timePattern.Match(rawTime).Groups[8].Value;


        }

        static int SetDaysOfWeek(string rawTime)
        {
            Regex timePattern = new Regex(@"^(TBA|[MTWRFSU])([MTWRFSU]?)([MTWRFSU]?)([MTWRFSU]?)\s(\d{1,4})([AP]?M?)-(\d{1,4})([AP]?M?)");
            int daysOfWeekBits = 0;
            string currentDay;
            for (int i = 1; i < 5; i++)
            {
                currentDay = timePattern.Match(rawTime).Groups[i].Value.Trim().ToLower();
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
            Regex timePattern = new Regex(@"^(TBA|[MTWRFSU])([MTWRFSU]?)([MTWRFSU]?)([MTWRFSU]?)\s(\d{1,4})([AP]?M?)-(\d{1,4})([AP]?M?)");
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
