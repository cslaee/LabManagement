using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabManagement
{
    class Common
    {
        public static void DebugWriteLine(bool isDebug, string message)
        {
            if (isDebug)
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
                Console.WriteLine("   * " + methodBase.Name + "() " + message);
            }
        }
        public static void DebugWrite(bool isDebug, string message)
        {
            if (isDebug)
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
                Console.Write("*" + methodBase.Name + "()" + message);
            }
        }

        public static string GetFileName()
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
            }
            return filePath;
        }

        internal static void DebugWriteLine(bool debug, object p)
        {
            throw new NotImplementedException();
        }

        //public static string GetMonthDayString(Regex dateRegex, string dateRange, int dateIndex)
        //{
        //    string monthStr = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dateRegex.Match(dateRange).Groups[dateIndex].Value.ToLower());
        //    string dayStr = dateRegex.Match(dateRange).Groups[dateIndex + 1].Value;

        //    bool hasMonth = monthStr.Length > 2;
        //    if (hasMonth)
        //    {
        //        string monthShortStr = monthStr.Substring(0, 3);
        //        int monthInt = DateTime.ParseExact(monthShortStr, "MMM", CultureInfo.InvariantCulture).Month;
        //        return "-" + monthInt + "-" + dayStr;
        //    }
        //    return "";
        //}



    }
}

