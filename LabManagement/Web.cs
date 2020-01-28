using System.IO;
using System.Text;
namespace LabManagement
{
    class Web
    {
//        static string workingDirectory = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, @"..\..\"));
       
        static readonly bool debug = Constants.webDebug;
        static public void PublishSchedule()
        {
            Common.DebugWriteLine(debug, "Web.PublishSchedule()");
            string semesterNamesSQL = @"SELECT DISTINCT substr(name, 1, 3) || ' ' || substr(year, 3, 4) FROM Semester " +
                                      "INNER JOIN SemesterName ON SemesterName.semesterNameID = Semester.nameFK ORDER BY year DESC, nameFK DESC";

            var tuple = Db.GetTuple(semesterNamesSQL);
            Common.DebugWriteLine(debug, "Number of Semesters = " + tuple.Length);
            StoreTabStrip(tuple, "index");
            StoreFileList(tuple.Length, "index");
            StoreIndex(tuple, "index");
        }


        static public void StoreTabStrip(string[] semesterNames, string sheetName)
        {
            #region html Content Strings
            string[] header = new[]  {"<html>","<head>", "<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">",
                "<meta name=ProgId content=Excel.Sheet>", "<meta name=Generator content=\"Microsoft Excel 15\">",
                "<link id=Main-File rel=Main-File href=\"../" + sheetName + ".htm\">", "", "<script language=\"JavaScript\">", "<!--",
                "if (window.name!=\"frTabs\")", " window.location.replace(document.all.item(\"Main-File\").href);", "//-->", "</script>",
                "<style>", "<!--", "A {", "    text-decoration:none;", "    color:#000000;", "    font-size:9pt;", "}", "-->", 
                "</style>", "</head>", "<body topmargin=0 leftmargin=0 bgcolor=\"#808080\">", "<table border=0 cellspacing=1>", " <tr>"};
            string[] footer = new[] { "", " </tr>", "</table>", "</body>", "</html>" };

            string a = " <td bgcolor=\"#";
            string b = "\" nowrap><b><small><small>&nbsp;<a href=\"";
            string c = "\" target=\"frSheet\"><font face=\"Arial\" color=\"#";
            string d = "\">";
            string e = "</font></a>&nbsp;</small></small></b></td>";

            string tabColor = "FFFFFF";
            string textColor = "000000";
            string linkName = "sheet001.htm";
            #endregion

            using (FileStream fs = new FileStream(Constants.webpageDir + sheetName + @"_files\tabstrip.htm", FileMode.Create))
//            using (FileStream fs = new FileStream(Constants.webpageDir + @"index_files\tabstrip.htm", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    WriteToFile(header, w);
                    foreach (string tabName in semesterNames)
                    {
                        Common.DebugWriteLine(debug, tabName);
                        w.WriteLine(a + tabColor + b + linkName + c + textColor + d + tabName + e);
                    }
                    WriteToFile(footer, w);
                }
            }

        }

        static public void StoreFileList(int numberOfSchedules, string sheetName)
        {
            #region html Content Strings
            string currentSheet; 
            string[] header = new[] { "<xml xmlns:o=\"urn:schemas-microsoft-com:office:office\">", 
                " <o:MainFile HRef=\"../" + sheetName + ".htm\"/>",
                " <o:File HRef=\"stylesheet.css\"/>",
                " <o:File HRef=\"tabstrip.htm\"/>"};
            string[] footer = new[] { " <o:File HRef=\"filelist.xml\"/>",
                "</xml>" };
            #endregion

            using (FileStream fs = new FileStream(Constants.webpageDir + sheetName + @"_files\filelist.xml", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    WriteToFile(header, w);
                    for (int i = 1; i <= numberOfSchedules; i++)
                    {
                        currentSheet = "sheet" + i.ToString("000") + ".htm";
                        Common.DebugWriteLine(debug, currentSheet);
                        w.WriteLine(" <o:File HRef=\"" + currentSheet + "\"/>" );
                    }
                    WriteToFile(footer, w);
                }
            }

        }

        static public void StoreIndex(string[] semesterNames, string sheetName)
        {
            int numberOfSheets = semesterNames.Length;
            #region html Content Strings
            string currentSheet;
            int i;
            string[] header = new[]  {"<html xmlns:v=\"urn:schemas-microsoft-com:vml\"", "xmlns:o=\"urn:schemas-microsoft-com:office:office\"",
                "xmlns:x=\"urn:schemas-microsoft-com:office:excel\"", "xmlns=\"http://www.w3.org/TR/REC-html40\">", "", "<head>",
                "<meta name=\"Excel Workbook Frameset\">", "<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">",
                "<meta name=ProgId content=Excel.Sheet>", "<meta name=Generator content=\"Microsoft Excel 15\">",
                "<link rel=File-List href=\"index_files/filelist.xml\">", "<title>Class Schedule for Electrical Engineering</title>",
                "<![if !supportTabStrip]>"};
            string[] footer = new[] { "", "<link id=\"shLink\">", "", "<script language=\"JavaScript\">", "<!--",
                " var c_lTabs=" + numberOfSheets +";", "", " var c_rgszSh=new Array(c_lTabs);"};
            string[] arrays = new[] {"","",""," var c_rgszClr=new Array(8);"," c_rgszClr[0]=\"window\";"," c_rgszClr[1]=\"buttonface\";",
                " c_rgszClr[2]=\"windowframe\";"," c_rgszClr[3]=\"windowtext\";"," c_rgszClr[4]=\"threedlightshadow\";",
                " c_rgszClr[5]=\"threedhighlight\";"," c_rgszClr[6]=\"threeddarkshadow\";"," c_rgszClr[7]=\"threedshadow\";","",
                " var g_iShCur;"," var g_rglTabX=new Array(c_lTabs);" };
            string[] fnGetIEVer = new[] { "", "function fnGetIEVer()", "{", " var ua=window.navigator.userAgent", " var msie=ua.indexOf(\"MSIE\")",
                " if (msie>0 && window.navigator.platform==\"Win32\")", "  return parseInt(ua.substring(msie+5,ua.indexOf(\".\", msie)));",
                " else", "  return 0;","}" };
            string[] fnBuildFrameset = new[] { "", "function fnBuildFrameset()", "{", " var szHTML=\"<frameset rows=\\\"*,18\\\""+
                " border=0 width=0 frameborder=no framespacing=0>\"+", 
                "  \"<frame src=\\\"\"+document.all.item(\"shLink\")[1].href+\"\\\" name=\\\"frSheet\\\" noresize>\"+",
                "  \"<frameset cols=\"54,*\" border=0 width=0 frameborder=no framespacing=0>\"",
                "  \"<frame src=\"\" name=\"frScroll\" marginwidth=0 marginheight=0 scrolling=no>\"",
                "  \"<frame src=\\\"\\\" name=\\\"frTabs\\\" marginwidth=0 marginheight=0 scrolling=no>\"+", 
                "  \"</frameset></frameset><plaintext>\";", "", 
                " with (document) {", "  open(\"text/html\",\"replace\");", "  write(szHTML);", "  close();", " }", "",
                " fnBuildTabStrip();", "}", "" };
            string[] fnBuildTabStrip = new[] { "function fnBuildTabStrip()", "{", " var szHTML=", 
                "  \"<html><head><style>.clScroll {font:8pt Courier New;color:\"+c_rgszClr[6]+\";cursor:default;line-height:10pt;}\"+",
                "  \".clScroll2 {font:10pt Arial;color:\"+c_rgszClr[6]+\";cursor:default;line-height:11pt;}</style></head>\"+",
                "  \"<body onclick=\\\"event.returnValue=false;\\\" ondragstart=\\\"event.returnValue=false;\\\""+
                " onselectstart=\\\"event.returnValue=false;\\\" bgcolor=\"+c_rgszClr[4]+\" topmargin=0"+
                " leftmargin=0><table cellpadding=0 cellspacing=0 width=100%>\"+",
                "  \"<tr><td colspan=6 height=1 bgcolor=\"+c_rgszClr[2]+\"></td></tr>\"+", 
                "  \"<tr><td style=\\\"font:1pt\\\">&nbsp;<td>\"+",
                "  \"<td valign=top id=tdScroll class=\\\"clScroll\\\" onclick=\\\"parent.fnFastScrollTabs(0);\\\""+
                " onmouseover=\\\"parent.fnMouseOverScroll(0);\\\" onmouseout=\\\"parent.fnMouseOutScroll(0);\\\"><a>&#171;</a></td>\"+",
                "  \"<td valign=top id=tdScroll class=\\\"clScroll2\\\" onclick=\\\"parent.fnScrollTabs(0);\\\" ondblclick=\\\"parent.fnScrollTabs(0);\\\""+
                " onmouseover=\\\"parent.fnMouseOverScroll(1);\\\" onmouseout=\\\"parent.fnMouseOutScroll(1);\\\"><a>&lt</a></td>\"+",
                "  \"<td valign=top id=tdScroll class=\\\"clScroll2\\\" onclick=\\\"parent.fnScrollTabs(1);\\\" ondblclick=\\\"parent.fnScrollTabs(1);\\\""+
                " onmouseover=\\\"parent.fnMouseOverScroll(2);\\\" onmouseout=\\\"parent.fnMouseOutScroll(2);\\\"><a>&gt</a></td>\"+",
                "  \"<td valign=top id=tdScroll class=\\\"clScroll\\\" onclick=\\\"parent.fnFastScrollTabs(1);\\\" onmouseover=\\\""+
                "parent.fnMouseOverScroll(3);\\\" onmouseout=\\\"parent.fnMouseOutScroll(3);\\\"><a>&#187;</a></td>\"+",
                "  \"<td style=\\\"font:1pt\\\">&nbsp;<td></tr></table></body></html>\";", 
                "", " with (frames['frScroll'].document) {", 
                "  open(\"text/html\",\"replace\");",
                "  write(szHTML);", 
                "  close();", " }", "", 
                " szHTML =", "  \"<html><head>\"+", 
                "  \"<style>A:link,A:visited,A:active {text-decoration:none;\"+\"color:\"+c_rgszClr[3]+\";}\"+",
                "  \".clTab {cursor:hand;background:\"+c_rgszClr[1]+\";font:9pt Arial;padding-left:3px;padding-right:3px;text-align:center;}\"+",
                "  \".clBorder {background:\"+c_rgszClr[2]+\";font:1pt;}\"+",
                "  \"</style></head><body onload=\\\"parent.fnInit();\\\" onselectstart=\\\"event.returnValue=false;\\\" ondragstart=\\\""+
                "event.returnValue=false;\\\" bgcolor=\"+c_rgszClr[4]+",
                "  \" topmargin=0 leftmargin=0><table id=tbTabs cellpadding=0 cellspacing=0>\";", "",
                " var iCellCount=(c_lTabs+1)*2;","",
                " var i;"," for (i=0;i<iCellCount;i+=2)",
                "  szHTML+=\"<col width=1><col>\";","",
                " var iRow;"," for (iRow=0;iRow<6;iRow++) {","",
                "  szHTML+=\"<tr>\";","",
                "  if (iRow==5)",
                "   szHTML+=\"<td colspan=\"+iCellCount+\"></td>\";", 
                "  else {", "   if (iRow==0) {", "    for(i=0;i<iCellCount;i++)",
                "     szHTML+=\"<td height=1 class=\\\"clBorder\\\"></td>\";", 
                "   } else if (iRow==1) {", "    for(i=0;i<c_lTabs;i++) {",
                "     szHTML+=\"<td height=1 nowrap class=\\\"clBorder\\\">&nbsp;</td>\";", 
                "     szHTML+=",
                "      \"<td id=tdTab height=1 nowrap class=\\\"clTab\\\" onmouseover=\\\"parent.fnMouseOverTab(\"+i+\");\\\" onmouseout=\\\""+
                "parent.fnMouseOutTab(\"+i+\");\\\">\"+",
                "      \"<a href=\\\"\"+document.all.item(\"shLink\")[i].href+\"\\\" target=\\\"frSheet\\\" id=aTab>&nbsp;\"+c_rgszSh[i]+\""+
                "&nbsp;</a></td>\";", "    }",
                "    szHTML+=\"<td id=tdTab height=1 nowrap class=\\\"clBorder\\\"><a id=aTab>&nbsp;</a></td><td width=100%></td>\";", 
                "   } else if (iRow==2) {", "    for (i=0;i<c_lTabs;i++)", 
                "     szHTML+=\"<td height=1></td><td height=1 class=\\\"clBorder\\\"></td>\";",
                "    szHTML+=\"<td height=1></td><td height=1></td>\";", "   } else if (iRow==3) {", 
                "    for (i=0;i<iCellCount;i++)", "     szHTML+=\"<td height=1></td>\";",
                "   } else if (iRow==4) {", "    for (i=0;i<c_lTabs;i++)", "     szHTML+=\"<td height=1 width=1></td><td height=1></td>\";",
                "    szHTML+=\"<td height=1 width=1></td><td></td>\";", "   }", "  }", "  szHTML+=\"</tr>\";", " }", "", 
                " szHTML+=\"</table></body></html>\";",
                " with (frames['frTabs'].document) {", "  open(\"text/html\",\"replace\");", "  charset=document.charset;", "  write(szHTML);",
                "  close();", " }", "}", "" };
            string[] fnInit = new[] {"function fnInit()",
                "{"," g_rglTabX[0]=0;"," var i;",
                " for (i=1;i<=c_lTabs;i++)",
                "  with (frames['frTabs'].document.all.tbTabs.rows[1].cells[fnTabToCol(i-1)])",
                "   g_rglTabX[i]=offsetLeft+offsetWidth-6;","}","",
                "function fnTabToCol(iTab)",
                "{"," return 2*iTab+1;","}",""};
            string[] fnNextTab = new[] { "function fnNextTab(fDir)",
                "{", " var iNextTab=-1;", " var i;", "",
                " with (frames['frTabs'].document.body) {",
                "  if (fDir==0) {", "   if (scrollLeft>0) {",
                "    for (i=0;i<c_lTabs&&g_rglTabX[i]<scrollLeft;i++);",
                "    if (i<c_lTabs)", "     iNextTab=i-1;", "   }",
                "  } else {", "   if (g_rglTabX[c_lTabs]+6>offsetWidth+scrollLeft) {",
                "    for (i=0;i<c_lTabs&&g_rglTabX[i]<=scrollLeft;i++);",
                "    if (i<c_lTabs)", "     iNextTab=i;", "   }", "  }", " }", " return iNextTab;", "}", "" };
            string[] fnScrollTabs = new[] {"function fnScrollTabs(fDir)",
                "{"," var iNextTab=fnNextTab(fDir);",""," if (iNextTab>=0) {","  frames['frTabs'].scroll(g_rglTabX[iNextTab],0);",
                "  return true;"," } else","  return false;","}","",
                "function fnFastScrollTabs(fDir)","{"," if (c_lTabs>16)",
                "  frames['frTabs'].scroll(g_rglTabX[fDir?c_lTabs-1:0],0);"," else",
                "  if (fnScrollTabs(fDir)>0) window.setTimeout(\"fnFastScrollTabs(\"+fDir+\");\",5);",
                "}","" };
            string[] fnSetTabProps = new[] { "function fnSetTabProps(iTab,fActive)", "{",
                " var iCol=fnTabToCol(iTab);", " var i;", "", " if (iTab>=0) {", "  with (frames['frTabs'].document.all) {",
                "   with (tbTabs) {", "    for (i=0;i<=4;i++) {", "     with (rows[i]) {", "      if (i==0)",
                "       cells[iCol].style.background=c_rgszClr[fActive?0:2];",
                "      else if (i>0 && i<4) {", "       if (fActive) {", "        cells[iCol-1].style.background=c_rgszClr[2];",
                "        cells[iCol].style.background=c_rgszClr[0];", "        cells[iCol+1].style.background=c_rgszClr[2];",
                "       } else {", "        if (i==1) {", "         cells[iCol-1].style.background=c_rgszClr[2];",
                "         cells[iCol].style.background=c_rgszClr[1];", "         cells[iCol+1].style.background=c_rgszClr[2];",
                "        } else {", "         cells[iCol-1].style.background=c_rgszClr[4];",
                "         cells[iCol].style.background=c_rgszClr[(i==2)?2:4];",
                "         cells[iCol+1].style.background=c_rgszClr[4];", "        }", "       }", "      } else",
                "       cells[iCol].style.background=c_rgszClr[fActive?2:4];",
                "     }", "    }", "   }", "   with (aTab[iTab].style) {", "    cursor=(fActive?\"default\":\"hand\");",
                "    color=c_rgszClr[3];", "   }", "  }", " }", "}", "" };
 
            string[] fnMouseOverScroll = new[] {"function fnMouseOverScroll(iCtl)","{",
                " frames['frScroll'].document.all.tdScroll[iCtl].style.color=c_rgszClr[7];","}","",
                "function fnMouseOutScroll(iCtl)",
                "{"," frames['frScroll'].document.all.tdScroll[iCtl].style.color=c_rgszClr[6];","}","",
                "function fnMouseOverTab(iTab)",
                "{"," if (iTab!=g_iShCur) {",
                "  var iCol=fnTabToCol(iTab);","  with (frames['frTabs'].document.all) {",
                "   tdTab[iTab].style.background=c_rgszClr[5];","  }"," }","}","",
                "function fnMouseOutTab(iTab)",
                "{"," if (iTab>=0) {","  var elFrom=frames['frTabs'].event.srcElement;",
                "  var elTo=frames['frTabs'].event.toElement;","","  if ((!elTo) ||",
                "   (elFrom.tagName==elTo.tagName) ||","   (elTo.tagName==\"A\" && elTo.parentElement!=elFrom) ||",
                "   (elFrom.tagName==\"A\" && elFrom.parentElement!=elTo)) {","",
                "   if (iTab!=g_iShCur) {","    with (frames['frTabs'].document.all) {",
                "     tdTab[iTab].style.background=c_rgszClr[1];","    }","   }","  }"," }","}","",
                "function fnSetActiveSheet(iSh)",
                "{"," if (iSh!=g_iShCur) {", "  fnSetTabProps(g_iShCur,false);",
                "  fnSetTabProps(iSh,true);","  g_iShCur=iSh;"," }","}","",
                " window.g_iIEVer=fnGetIEVer();",
                " if (window.g_iIEVer>=4)","  fnBuildFrameset();","//-->","</script>","<![endif]><!--[if gte mso 9]><xml>",
                " <x:ExcelWorkbook>","  <x:ExcelWorksheets>"};

            string[] bottom = new[] {"</x:ExcelWorksheets>", "  <x:Stylesheet HRef=\"schedule_files/stylesheet.css\"/>",
                "  <x:WindowHeight>12375</x:WindowHeight>", "  <x:WindowWidth>16320</x:WindowWidth>",
                "  <x:WindowTopX>5250</x:WindowTopX>", "  <x:WindowTopY>75</x:WindowTopY>",
                "  <x:TabRatio>734</x:TabRatio>", "  <x:ActiveSheet>1</x:ActiveSheet>",
                "  <x:ProtectStructure>False</x:ProtectStructure>", "  <x:ProtectWindows>False</x:ProtectWindows>",
                " </x:ExcelWorkbook>", "</xml><![endif]-->", "</head>", "",
                "<frameset rows=\"*,39\" border=0 width=0 frameborder=no framespacing=0>",
                " <frame src=\"schedule_files/sheet002.htm\" name=\"frSheet\">",
                " <frame src=\"schedule_files/tabstrip.htm\" name=\"frTabs\" marginwidth=0 marginheight=0>",
                " <noframes>", "  <body>", "   <p>This page uses frames, but your browser doesn't support them.</p>",
                "  </body>", " </noframes>", "</frameset>", "</html>"};
           
            #endregion

            using (FileStream fs = new FileStream(Constants.webpageDir + sheetName + ".htm", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    WriteToFile(header, w);
                    for (i = 1; i <= numberOfSheets; i++)
                    {
                        currentSheet = sheetName + "_files/sheet" + i.ToString("000") + ".htm";
                        Common.DebugWriteLine(debug, currentSheet);
                        w.WriteLine("<link id=\"shLink\" href=" + currentSheet + "\"/>" );
                    }

                    WriteToFile(footer, w);
                    i = 0;                    
                    foreach (string tabName in semesterNames)
                    {
                        Common.DebugWriteLine(debug, tabName);
                        w.WriteLine(" c_rgszSh[" + i + "] = \"" + tabName + "\";");
                        i++;
                    }

                    WriteToFile(arrays, w);
                    WriteToFile(fnGetIEVer, w);
                    WriteToFile(fnBuildFrameset , w);
                    WriteToFile(fnBuildTabStrip , w);
                    WriteToFile(fnInit, w);
                    WriteToFile(fnNextTab, w);
                    WriteToFile(fnScrollTabs, w);
                    WriteToFile(fnSetTabProps , w);
                    WriteToFile(fnMouseOverScroll , w);
                    i = 1;
                    
                    foreach (string tabName in semesterNames)
                    {
                        Common.DebugWriteLine(debug, tabName);
                        currentSheet = "sheet" + i.ToString("000") + ".htm";
                        w.WriteLine("   <x:ExcelWorksheet>");
                        w.WriteLine("    <x:Name>" + tabName + "</x:Name>");
                        w.WriteLine("    <x:WorksheetSource HRef=" + sheetName + "_files/" + currentSheet + "\"/>");
                        w.WriteLine("   </x:ExcelWorksheet>");
                        i++;
                    }
                   
                    WriteToFile(bottom, w);
                }
            }

        }



        static public void StoreSheets()
        {
            #region html Content Strings
            string[] header = new[]  {"<html>","<head>", "<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">",
                "<meta name=ProgId content=Excel.Sheet>", "<meta name=Generator content=\"Microsoft Excel 15\">",
                "<link id=Main-File rel=Main-File href=\"../index.htm\">", "<script language=\"JavaScript\">", "<!--", "if (window.name!=\"frTabs\")",
                " window.location.replace(document.all.item(\"Main-File\").href);", "//-->", "</script>", "<style>", "<!--", "A {",
                "    text-decoration:none;", "    color:#000000;", "    font-size:9pt;", "}", "-->", "</style>", "</head>",
                "<body topmargin=0 leftmargin=0 bgcolor=\"#808080\">", "<table border=0 cellspacing=1>", " <tr>"};
            string[] footer = new[] { " </tr>", "</table>", "</body>", "</html>" };

            string a = "<td bgcolor=\"#";
            string b = "\" nowrap><b><small><small>&nbsp;<a href=\"";
            string c = "\" target=\"frSheet\"><font face=\"Arial\" color=\"#";
            string d = "\">";
            string e = "</font></a>&nbsp;</small></small></b></td>";

            string tabColor = "FFFFFF";
            string textColor = "000000";
            string linkName = "sheet001.htm";
            #endregion


            string semesterNamesSQL = @"SELECT DISTINCT substr(name, 1, 3) || ' ' || substr(year, 3, 4) FROM Semester " +
                                      "INNER JOIN SemesterName ON SemesterName.semesterNameID = Semester.nameFK ORDER BY year DESC, nameFK DESC";
            Common.DebugWriteLine(debug, "Web Debug");

            var tuple = Db.GetTuple(semesterNamesSQL);


            using (FileStream fs = new FileStream(Constants.webpageDir + @"index_files\sheet.htm", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    WriteToFile(header, w);
                    foreach (string tabName in tuple)
                    {
                        Common.DebugWriteLine(debug, tabName);
                        w.WriteLine(a + tabColor + b + linkName + c + textColor + d + tabName + e);
                    }
                    WriteToFile(footer, w);
                }
            }

        }

        static void WriteToFile(string[] content, StreamWriter w)
        {
            foreach (string item in content)
            {
                w.WriteLine(item);
            }
        }


    }
}
