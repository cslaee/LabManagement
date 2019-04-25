using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ImportSchedule is firing this
namespace LabManagement
{
    class Web
    {

        static readonly bool debug = Constants.webDebug;
        static public void PublishSchedule()
        {
            string semesterNamesSQL = @"SELECT DISTINCT substr(name, 1, 3) || ' ' || substr(year, 3, 4) FROM Semester INNER JOIN SemesterName ON SemesterName.semesterNameID = Semester.nameFK ORDER BY year DESC, nameFK DESC";
            Common.DebugWriteLine(debug, "Web Debug");

            var tuple = Db.GetTuple(semesterNamesSQL);
            TabStripTop(tuple);
        }


        static public void TabStripTop(string[] semesterNames)
        {

            string[] header = new[]  {"<html>","<head>", "<meta http-equiv=Content-Type content=\"text / html; charset = windows - 1252\">",
                "<meta name=ProgId content=Excel.Sheet>", "<meta name=Generator content=\"Microsoft Excel 15\">",
                "<link id=Main-File rel=Main-File href=\"../index.htm\">", "<script language=\"JavaScript\">", "<!--", "if (window.name != \"frTabs\")",
                "window.location.replace(document.all.item(\"Main-File\").href);", "//-->", "</script>", "<style>", "< !--", "A {",
                "    text - decoration:none;", "    color:#000000;", "    font - size:9pt;", "}", "-->", "</style>", "</head>",
                "<body topmargin = 0 leftmargin = 0 bgcolor = \"#808080\">", "<table border=0 cellspacing=1>", " <tr>"};

            string[] footer = new[] { " </tr>", "</table>", "</body>", "</html>" };

            string a = "<td bgcolor=\"#";
            string b = "\" nowrap><b><small><small>&nbsp;<a href=\"";
            string c = "\" target=\"frSheet\"><font face=\"Arial\" color=\"#";
            string d = "\">";
            string e = "</font></a>&nbsp;</small></small></b></td>";

            string tabColor = "FFFFFF";
            string textColor = "000000";
            string linkName = "sheet001.htm";
            string tabNameOld = "sum20";

            using (FileStream fs = new FileStream(Constants.webpageDir + "tabstrip.htm", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (string item in header)
                    {
                        w.WriteLine(item);
                    }

                    foreach (string tabName in semesterNames)
                    {
                        Common.DebugWriteLine(debug, tabName);
                        w.WriteLine(a + tabColor + b + linkName + c + textColor + d + tabName + e);
                    }


                    w.WriteLine(a + tabColor + b + linkName + c + textColor + d + tabNameOld + e);

                    foreach (string item in footer)
                    {
                        w.WriteLine(item);
                    }
                }
            }

        }
    }
}
