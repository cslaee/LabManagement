﻿using System;
using System.Text.RegularExpressions;

namespace LabManagement
{
    class Constants
    {
        public const bool wipeDB = false;
    
        public const bool dbDebug = false;
        public const bool importScheduleDebug = true;
        public const bool semesterDebug = false;
        public const bool calendarDebug = true;
        public const bool courseDebug = false;
        public const bool roomDebug = false;
        public const bool schedule = false;

        public const string username = "John Doe";
        public const string email = "test@test.test";
        public const string databaseName = "CalStateLAeeDB.sqlite3";
        public const string connectionString = @"Data Source=|DataDirectory|" + databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";

        //    static string connectionString = @"Data Source=" + Constants.databaseName + "; Version=3; FailIfMissing=True; Foreign Keys=True;";
        public const string locksJsonFileName = "Locks.json";
        public const string sqlFileName = "db.sql";
        public string workingDirectory = System.AppContext.BaseDirectory;
        public const string webpageDir = @"C:\Users\moberme\Documents\LabManagement\webpage\index_files\";

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

        public const string coursePattern = @"([A-Z]{1,4})\s?(\d{4})-?(\d{0,2})";
        public const string userPattern = @"(\w+)\/?(\w+)?";
        public const string roomPattern = @"^(ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?([A-F]|LH)?(\d{1,4})([A-G])?\/?((ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?([A-F]|LH)?(\d{1,4})([A-G])?)?";



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


