using System.IO;

namespace LabManagement
{
    class Constants
    {
        public const bool wipeDB = true;
        public const bool dbDebug = false;
        public const bool importScheduleDebug = false;
        public const bool semesterDebug = false;
        public const bool calendarDebug = false;
        public const bool courseDebug = false;
        public const bool roomDebug = false;
        public const bool schedule = false;
        public const bool webDebug = true;

        public const string username = "John Doe";
        public const string email = "test@test.test";
        public const string databaseName = "CalStateLAeeDB.sqlite3";
        public const string connectionString = @"Data Source=|DataDirectory|" + databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";

        public const string locksJsonFileName = "Locks.json";

        public static string workingDirectory = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\"));
        public static string webpageDir = workingDirectory + @"webpage\"; 
        public static string sqlPathAndFileName = workingDirectory + "db.sql";
        public static string excelInitialDataPathAndFileName = workingDirectory + "InitialData.xlsx";


        public const string dashPattern = @"(\s?-\s?)";
        public const string yearPattern = @"(\d{4})";
        public const string dayYearPattern = @"\s(\d{1,2}),\s" + yearPattern;
        public const string datePattern = @"(\d{1,2})\/(\d{1,2})\/" + yearPattern;
        public const string revisionDatePattern = datePattern + "(?!-)";
        public const string seasonPattern = @"(?i)(FALL|WINTER|SPRING|SUMMER)";
        public const string semesterNameAndYearPattern = seasonPattern + @"\s" + yearPattern;
        public const string monthPattern = @"(\b(?i)(?:jan(?:uary)?|feb(?:ruary)?|mar(?:ch)?|apr(?:il)?|may|jun(?:e)?|jul(?:y)?|aug(?:ust)?|sep(?:tember)?|oct(?:ober)?|nov(?:ember)?|dec(?:ember)?))";
        public const string semesterDateRangePattern = monthPattern + dayYearPattern + dashPattern + monthPattern + dayYearPattern;
        public const string summerDateRangePattern = datePattern + dashPattern + datePattern;
        public const string summerDateRangeNoYearPattern = datePattern + dashPattern + datePattern;

        public const string toPattern = @"(?i)(\s?to\s?)";
        public const string sessionWeekPattern = @"Session\s([A,B,C]):\s(\d{1,2})\s(?i)(WEEK\sSESSION,\s)";
        public const string monthDayPattern = monthPattern + @"\s(\d{1,2})"; 
        public const string summerSessionABCPattern = sessionWeekPattern + monthDayPattern + toPattern + monthDayPattern; //https://regex101.com/r/2F40G2/5/

        public const string coursePattern = @"([A-Z]{1,4})\s?(\d{4})-?(\d{0,2})";
        public const string userPattern = @"(\w+)\/?(\w+)?";

        public const string buildingPattern = @"(ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?";
        public const string roomNumberPattern = @"([A-F]|LH)?(\d{1,4})([A-G])?";
        public const string roomPattern = "^" + buildingPattern + roomNumberPattern + @"\/?(" + buildingPattern + roomNumberPattern + ")?";



    }
    // ** useful. Passed from a this obj
    //  obj.GetType().GetProperty("year").SetValue(obj, 9999, null);// pretty cool
    //foreach (var prop in obj.GetType().GetProperties())
    //{
    //    Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(obj, null));
    //}

}
//todo Add Edit User Panel
//todo Add Edit Course Panel
//todo Add Edit Room Panel
//todo Build Send email to instructor button

//todo Does Class DefaultRoom match Schedule Room?
//todo Does Class maxSections match Schedule Section?


