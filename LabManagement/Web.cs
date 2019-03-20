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
        static public void BuildSchedule()
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
            string tabName = "sum20";

            using (FileStream fs = new FileStream(Constants.webpageDir + "tabstrip.htm", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach(string item in header)
                    {
                    w.WriteLine(item);
                    }

                    
                    w.WriteLine(a + tabColor + b + linkName + c + textColor + d + tabName + e);

                    foreach(string item in footer)
                    {
                    w.WriteLine(item);
                    }
                }
            }
        }

    }
}
