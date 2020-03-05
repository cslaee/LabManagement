using System;
using System.Collections.Generic;
using System.Globalization;
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

           string semesterNamesSQL = @"SELECT DISTINCT substr(name, 1, 3) ||  substr(year, 3, 4), name, session, numberOfWeeks, semesterID, version, year, scheduleDate, schedulePostDate  FROM Semester " +
                                      "INNER JOIN SemesterName ON SemesterName.semesterNameID = Semester.nameFK ORDER BY year DESC, nameFK DESC";
            var tuple = Db.GetTuple(semesterNamesSQL);
            List <object> tuple7 = Db.GetTupleNewOne(semesterNamesSQL);

            List<Semester> semesterList = Semester.GetSemesterList();
            //List<List<object>> tupleO = Db.GetTupleObj(semesterNamesSQL);
            Common.DebugWriteLine(debug, "Number of Semesters = " + tuple.Length);
            Common.DebugWriteLine(debug, "Number of Semesters 7 = " + tuple7.Count);
            Common.DebugWriteLine(debug, " **********************************************************************************************************************");
            Common.DebugWriteLine(debug, " **********************************************************************************************************************");
            Common.DebugWriteLine(debug, " **********************************************************************************************************************");
//            Common.DebugWrite(debug, tuple7);
            Common.DebugWriteLine(debug, " **********************************************************************************************************************");
            Common.DebugWriteLine(debug, " **********************************************************************************************************************");
            Common.DebugWriteLine(debug, " **********************************************************************************************************************");


            int rowCount = tuple7.Count/9;
           
            string excelWorkSheet = "schedule";
            SetupDirectories(excelWorkSheet);
            StoreTabStrip(tuple, excelWorkSheet);
            StoreFileList(tuple, excelWorkSheet);
            StoreIndex(tuple, excelWorkSheet);
            StoreSheets(semesterList, excelWorkSheet);
            StoreStyleSheet(excelWorkSheet);
        
        }
        static public void SetupDirectories(string sheetName)
        {
            if (Directory.Exists(sheetName))  
            {  
                Directory.Delete(sheetName);  
            } 
             System.IO.Directory.CreateDirectory(Constants.webpageDir + sheetName + @"_files");
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
            #endregion

            string currentSheet;
            int i = 1;
            using (FileStream fs = new FileStream(Constants.webpageDir + sheetName + @"_files\tabstrip.htm", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    WriteToFile(header, w);
                    foreach (string tabName in semesterNames)
                    {
                        currentSheet = "sheet" + i.ToString("000") + ".htm";
                        Common.DebugWriteLine(debug, tabName);
                        w.WriteLine(a + tabColor + b + currentSheet + c + textColor + d + tabName + e);
                        i++;
                    }
                    WriteToFile(footer, w);
                }
            }

        }

        static public void StoreFileList(string[] semesterNames, string sheetName)
        {
            string currentSheet; 
            #region html Content Strings
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
                    for (int i = 1; i <= semesterNames.Length; i++)
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
            string currentSheet; 
            #region html Content Strings
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
                            w.WriteLine("<link id=\"shLink\" href=" + currentSheet + "\"/>");
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
                        WriteToFile(fnBuildFrameset, w);
                        WriteToFile(fnBuildTabStrip, w);
                        WriteToFile(fnInit, w);
                        WriteToFile(fnNextTab, w);
                        WriteToFile(fnScrollTabs, w);
                        WriteToFile(fnSetTabProps, w);
                        WriteToFile(fnMouseOverScroll, w);
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

        static public void TestStoreSheets(List<Semester> semesterList, int rowCount, string sheetName)
        {
            foreach (Semester s in semesterList)
            { 
                 Common.DebugWriteLine(debug, "NameYear =" + s.NameYear);
                 Common.DebugWriteLine(debug, "SemesterID =" + s.SemesterID);
                 Common.DebugWriteLine(debug, "Version =" + s.Version);
                 Common.DebugWriteLine(debug, "NameFK =" + s.NameFK);
                 Common.DebugWriteLine(debug, "Year =" + s.Year);
                 Common.DebugWriteLine(debug, "Name =" + s.Name);
            }
        }





        static public void StoreSheets(List<Semester> semesterList, string sheetName)
        {

            string currentSheet;
            string currentSemester;
            string currentYear = "1979";
            string subjectCatalogSection;
            string instructor;
            string room;
            int i = 1;
            #region html Content Strings

            string[] header = new[] { "<html xmlns:v=\"urn:schemas-microsoft-com:vml\"",
                "xmlns:o=\"urn:schemas-microsoft-com:office:office\"",
                "xmlns:x=\"urn:schemas-microsoft-com:office:excel\"", "xmlns=\"http://www.w3.org/TR/REC-html40\">",
                "", "<head>", "<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">",
                "<meta name=ProgId content=Excel.Sheet>", "<meta name=Generator content=\"Microsoft Excel 15\">",
                "<link id=Main-File rel=Main-File href=\"../" + sheetName + ".htm\">",
                "<link rel=File-List href=filelist.xml>", "<title>Class Schedule for Electrical Engineering</title>",
                "<link rel=Stylesheet href=stylesheet.css>", "<style>", "<!--table", "	{mso-displayed-decimal-separator:\"\\.\";",
                "	mso-displayed-thousand-separator:\"\\,\";}", "@page", "	{margin:.75in .25in .75in .25in;",
                "	mso-header-margin:.3in;", "	mso-footer-margin:.3in;", "	mso-page-orientation:landscape;}", "-->", "</style>" };

            string[] fnUpdateTabs = new[] { "<![if !supportTabStrip]><script language=\"JavaScript\">",
                "<!--", "function fnUpdateTabs()", " {", "  if (parent.window.g_iIEVer>=4) {",
                "   if (parent.document.readyState==\"complete\"", "    && parent.frames['frTabs'].document.readyState==\"complete\")",
                "   parent.fnSetActiveSheet(0);", "  else", "   window.setTimeout(\"fnUpdateTabs();\",150);",
                " }", "}", "", "if (window.name!=\"frSheet\")", " window.location.replace(\"../" + sheetName + ".htm\");",
                "else", " fnUpdateTabs();", "//-->", "</script>", "<![endif]>", "</head>", "", "<body link=blue vlink=purple class=xl155>","" };

            string[] table = new[] {"<table border=0 cellpadding=0 cellspacing=0 width=916 style='border-collapse:",
                " collapse;table-layout:fixed;width:688pt'>", " <col class=xl156 width=73 style='mso-width-source:userset;mso-width-alt:2669;",
                " width:55pt'>", " <col class=xl155 width=458 style='mso-width-source:userset;mso-width-alt:16749;", " width:344pt'>",
                " <col class=xl157 width=30 style='mso-width-source:userset;mso-width-alt:1097;", " width:23pt'>", 
                " <col class=xl155 width=103 style='mso-width-source:userset;mso-width-alt:3766;", " width:77pt'>", 
                " <col class=xl155 width=188 style='mso-width-source:userset;mso-width-alt:6875;", " width:141pt'>", 
                " <col class=xl155 width=64 style='mso-width-source:userset;mso-width-alt:2340;", " width:48pt'>" };

            string[] h1 = new[] {" <tr class=xl153 height=12 style='mso-height-source:userset;height:9.0pt'>",
                "  <td height=12 class=xl150 width=73 style='height:9.0pt;width:55pt'>Posted","  8/26/2019</td>",
                "  <td class=xl151 width=458 style='width:344pt'>&nbsp;</td>",
                "  <td class=xl152 width=30 style='width:23pt'>&nbsp;</td>",
                "  <td class=xl160 colspan=2 width=291 style='mso-ignore:colspan;width:218pt'><a",
                "  href=\"http://www.calstatela.edu/univ/ppa/acadcal.htm\" target=\"_parent\"><span",
                "  style='font-size:7.0pt;font-weight:700'>Click here to verify Key Dates</span></a></td>",
                "  <td rowspan=2 class=xl162 width=64 style='width:48pt'><a", 
                "  href=\"http://download.cslaee.com/\" target=\"_parent\"><span style='font-size:",
                "  7.0pt'>Download page to print</span></a></td>", " </tr>" };

            string[] h2 = new[] {"  <tr class=xl153 height=12 style='mso-height-source:userset;height:9.0pt'>",
                "  <td height=12 class=xl153 style='height:9.0pt'>&nbsp;</td>",
                "  <td class=xl152>&nbsp;</td>",
                "  <td class=xl152>&nbsp;</td>",
                "  <td class=xl149>August 19&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;",
                "  &nbsp; &nbsp;&nbsp;</td>",
                "  <td class=xl149>University Convocation, Fall semester begins</td>", " </tr>" };
            string[] footer = new[] { "</table>", "</body>", "</html>" };
            string[] h3 = new[] {" <tr class=xl153 height=12 style='mso-height-source:userset;height:9.0pt'>",
                "  <td height=12 class=xl153 style='height:9.0pt'>&nbsp;</td>",
                "  <td class=xl152>&nbsp;</td>",
                "  <td class=xl152>&nbsp;</td>",
                "  <td class=xl149>August 20</td>",
                "  <td class=xl149>Fall classes begin</td>",
                "  <td class=xl150>&nbsp;</td>",
                " </tr>",
                " <tr class=xl153 height=12 style='mso-height-source:userset;height:9.0pt'>",
                "  <td colspan=3 rowspan=3 height=36 class=xl161 style='height:27.0pt'><span",
                "  style='mso-spacerun:yes'> </span>ELECTRICAL AND COMPUTER ENGINEERING</td>",
                "  <td class=xl149>September 2</td>",
                "  <td class=xl149>Labor Day, University closed</td>",
                "  <td class=xl150>&nbsp;</td>",
                " </tr>",
                " <tr class=xl153 height=12 style='mso-height-source:userset;height:9.0pt'>",
                "  <td height=12 class=xl149 style='height:9.0pt'>November 11</td>",
                "  <td class=xl149>Veterans Day, University closed</td>",
                "  <td class=xl150>&nbsp;</td>",
                " </tr>",
                " <tr class=xl153 height=12 style='mso-height-source:userset;height:9.0pt'>",
                "  <td height=12 class=xl149 style='height:9.0pt'>November 25-27</td>",
                "  <td class=xl149>Fall Recess</td>",
                "  <td class=xl150>&nbsp;</td>",
                " </tr>",
                " <tr class=xl153 height=12 style='mso-height-source:userset;height:9.0pt'>" };
            string[] h4 = new[] {"  <td class=xl149>November 28-30</td>",
                "  <td class=xl149>Thanksgiving Holiday, University closed</td>",
                "  <td class=xl150>&nbsp;</td>",
                " </tr>",
                " <tr class=xl153 height=12 style='mso-height-source:userset;height:9.0pt'>",
                "  <td height=12 class=xl149 style='height:9.0pt'>December 10-16</td>",
                "  <td class=xl149>Final Examinations</td>",
                "  <td class=xl150>&nbsp;</td>",
                " </tr>",
                " <tr class=xl153 height=12 style='height:9.0pt'>",
                "  <td height=12 class=xl149 style='height:9.0pt'>December 20</td>",
                "  <td class=xl149>Fall semester ends</td>",
                "  <td class=xl150>&nbsp;</td>",
                " </tr>",
                " <tr class=xl153 height=12 style='height:9.0pt'>",
                "  <td height=12 class=xl152 style='height:9.0pt'>&nbsp;</td>",
                "  <td class=xl152>&nbsp;</td>",
                "  <td class=xl152>&nbsp;</td>",
                "  <td class=xl153>&nbsp;</td>",
                "  <td class=xl150>&nbsp;</td>",
                "  <td class=xl150>&nbsp;</td>",
                " </tr>",
                " <tr class=xl153 height=12 style='height:9.0pt'>",
                "  <td height=12 class=xl153 style='height:9.0pt'>&nbsp;</td>",
                "  <td class=xl153>&nbsp;</td>",
                "  <td class=xl154>&nbsp;</td>",
                "  <td class=xl148>&nbsp;</td>",
                "  <td class=xl153>&nbsp;</td>",
                "  <td class=xl153>&nbsp;</td>",
                " </tr>",
                " <tr class=xl153 height=13 style='height:9.75pt'>",
                "  <td height=13 class=xl158 style='height:9.75pt'>COURSE</td>",
                "  <td class=xl159>TITLE</td>",
                "  <td class=xl159>CR</td>",
                "  <td class=xl158>FACULTY</td>",
                "  <td class=xl158>DAYS/TIMES</td>",
                "  <td class=xl158>ROOM</td>",
                " </tr>"};

            string tdData = "  <td class=xl155>";

            string a = "<td bgcolor=\"#";
            string b = "\" nowrap><b><small><small>&nbsp;<a href=\"";
            string c = "\" target=\"frSheet\"><font face=\"Arial\" color=\"#";
            string d = "\">";
            string e = "</font></a>&nbsp;</small></small></b></td>";

            string tabColor = "FFFFFF";
            string textColor = "000000";
            string linkName = "sheet001.htm";
            #endregion



            foreach (Semester s in semesterList)
            { 
                 Common.DebugWriteLine(debug, "NameYear =" + s.NameYear);
                 Common.DebugWriteLine(debug, "SemesterID =" + s.SemesterID);
                 Common.DebugWriteLine(debug, "Version =" + s.Version);
                 Common.DebugWriteLine(debug, "NameFK =" + s.NameFK);
                 Common.DebugWriteLine(debug, "Year =" + s.Year);
                 Common.DebugWriteLine(debug, "Name =" + s.Name);
            }

            //for (int i = 0; i < rowCount; i++)
            foreach (Semester semester in semesterList)
            {
                currentSheet = Constants.webpageDir + sheetName + @"_files\sheet" + (i++).ToString("000") + ".htm";
                Common.DebugWriteLine(debug, "Sheet Name = " + semester.Name + " SemesterID = " + semester.SemesterID);

                using (FileStream fs = new FileStream(currentSheet, FileMode.Create))
                {
                    //currentSemester = semesterNames[i * 9 + 1].ToString().ToUpper();
                    //currentYear = semesterNames[i * 9 + 6].ToString();

                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    { 
                        WriteToFile(header, w);
                        WriteToFile(fnUpdateTabs, w);
                        WriteToFile(table, w);
                        WriteToFile(h1, w);
                        WriteToFile(h2, w);
                        WriteToFile(h3, w);
                        w.WriteLine("<td colspan=3 rowspan=3 height=36 class=xl161 style='height:27.0pt'>" + semester.NameYear.ToUpper());
                        w.WriteLine(" SEMESTER " + semester.Year + " COURSE LIST</td>");
                        WriteToFile(h4, w);
                        List<Schedule> semesterClassList = Schedule.GetSemsterClassList(semester.SemesterID);

                        foreach (Schedule courseTimeAndPlace in semesterClassList)
                        {
                            Course course = new Course(courseTimeAndPlace.CourseFK);
                            subjectCatalogSection = courseTimeAndPlace.Section == 0 ? course.Subject + 
                                course.Catalog : course.Subject + course.Catalog + "-" + courseTimeAndPlace.Section.ToString().PadLeft(2, '0');
                            User instructor1 = new User(courseTimeAndPlace.Instructor1FK);
                            User instructor2 = new User(courseTimeAndPlace.Instructor2FK);
                            instructor = instructor2.Last.Length == 0 ? instructor1.Last : instructor1.Last + "/" + instructor2.Last;
                            Room room1 = new Room(courseTimeAndPlace.Room1FK);
                            Room room2 = new Room(courseTimeAndPlace.Room2FK);
                            room = courseTimeAndPlace.Room2FK == 1 ? room1.BuildingWingNumberSub : room1.BuildingWingNumberSub + "/" +room2.BuildingWingNumberSub;
                            
                            Common.DebugWriteLine(debug, subjectCatalogSection + " | " + course.Title + " | " +
                                course.Credit + " | " + instructor + " | " + courseTimeAndPlace.DaysString + " | " + room);

                            w.WriteLine(" <tr height=15 style='height:11.25pt'>");
                            w.WriteLine(tdData + subjectCatalogSection + "</td>");
                            w.WriteLine(tdData + course.Title + "</td>");
                            w.WriteLine(tdData + course.Credit + "</td>");
                            w.WriteLine(tdData + instructor + "</td>");
                            w.WriteLine(tdData + courseTimeAndPlace.DaysString + "</td>");
                            w.WriteLine(tdData + room + "</td>");
                            w.WriteLine(" </tr>");
                        }
                            WriteToFile(footer, w);
                    }
                }

            }

        }


		static public void StoreStyleSheet(string sheetName)
		{
			#region html Content Strings
			string[] cssString = new[] {"tr",
				"	{mso-height-source:auto;}",
                "col",
                "	{mso-width-source:auto;}",
                "br",
                "	{mso-data-placement:same-cell;}",
                ".style126",
                "	{color:blue;",
                "	font-size:10.0pt;",
                "	font-weight:400;",
                "	font-style:normal;",
                "	text-decoration:underline;",
                "	text-underline-style:single;",
                "	font-family:Arial, sans-serif;",
                "	mso-font-charset:0;",
                "	mso-style-name:Hyperlink;",
                "	mso-style-id:8;}",
                "a:link",
                "	{color:blue;",
                "	font-size:10.0pt;",
                "	font-weight:400;",
                "	font-style:normal;",
                "	text-decoration:underline;",
                "	text-underline-style:single;",
                "	font-family:Arial, sans-serif;",
                "	mso-font-charset:0;}",
                "a:visited",
                "	{color:purple;",
                "	font-size:10.0pt;",
                "	font-weight:400;",
                "	font-style:normal;",
                "	text-decoration:underline;",
                "	text-underline-style:single;",
                "	font-family:Arial;",
                "	mso-generic-font-family:auto;",
                "	mso-font-charset:0;}",
                ".style0",
                "	{mso-number-format:General;",
                "	text-align:general;",
                "	vertical-align:bottom;",
                "	white-space:nowrap;",
                "	mso-rotate:0;",
                "	mso-background-source:auto;",
                "	mso-pattern:auto;",
                "	color:windowtext;",
                "	font-size:10.0pt;",
                "	font-weight:400;",
                "	font-style:normal;",
                "	text-decoration:none;",
                "	font-family:Arial;",
                "	mso-generic-font-family:auto;",
                "	mso-font-charset:0;",
                "	border:none;",
                "	mso-protection:locked visible;",
                "	mso-style-name:Normal;",
                "	mso-style-id:0;}",
                ".style113",
                "	{mso-number-format:General;",
                "	text-align:general;",
                "	vertical-align:bottom;",
                "	white-space:nowrap;",
                "	mso-rotate:0;",
                "	mso-background-source:auto;",
                "	mso-pattern:auto;",
                "	color:black;",
                "	font-size:11.0pt;",
                "	font-weight:400;",
                "	font-style:normal;",
                "	text-decoration:none;",
                "	font-family:Calibri, sans-serif;",
                "	mso-font-charset:0;",
                "	border:none;",
                "	mso-protection:locked visible;",
                "	mso-style-name:\"Normal 14\";}",
                "td",
                "	{mso-style-parent:style0;",
                "	padding-top:1px;",
                "	padding-right:1px;",
                "	padding-left:1px;",
                "	mso-ignore:padding;",
                "	color:windowtext;",
                "	font-size:10.0pt;",
                "	font-weight:400;",
                "	font-style:normal;",
                "	text-decoration:none;",
                "	font-family:Arial, sans-serif;",
                "	mso-font-charset:0;",
                "	mso-number-format:General;",
                "	text-align:general;",
                "	vertical-align:bottom;",
                "	border:none;",
                "	mso-background-source:auto;",
                "	mso-pattern:auto;",
                "	mso-protection:locked visible;",
                "	white-space:nowrap;",
                "	mso-rotate:0;",
                "	background:white;}",
                ".xl148",
                "	{mso-style-parent:style113;",
                "	font-size:7.0pt;",
                "	font-style:italic;",
                "	text-align:center;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl149",
                "	{mso-style-parent:style0;",
                "	font-size:7.0pt;",
                "	text-align:left;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl150",
                "	{mso-style-parent:style0;",
                "	font-size:7.0pt;",
                "	text-align:center;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl151",
                "	{mso-style-parent:style113;",
                "	color:black;",
                "	font-size:7.0pt;",
                "	mso-number-format:\"Short Date\";",
                "	text-align:left;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl152",
                "	{mso-style-parent:style113;",
                "	font-size:7.0pt;",
                "	font-weight:700;",
                "	font-style:italic;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl153",
                "	{mso-style-parent:style0;",
                "	font-size:7.0pt;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl154",
                "	{mso-style-parent:style113;",
                "	font-size:7.0pt;",
                "	font-weight:700;",
                "	font-style:italic;",
                "	text-align:center;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl155",
                "	{mso-style-parent:style0;",
                "	font-size:8.0pt;",
                "",
                "	mso-pattern:black none;}",
                ".xl156",
                "	{mso-style-parent:style0;",
                "	font-size:8.0pt;",
                "	text-align:left;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl157",
                "	{mso-style-parent:style0;",
                "	font-size:8.0pt;",
                "	text-align:center;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl158",
                "	{mso-style-parent:style113;",
                "	font-size:7.0pt;",
                "	font-weight:700;",
                "	font-style:italic;",
                "	text-align:left;",
                "	border-top:none;",
                "	border-right:none;",
                "	border-bottom:1.5pt solid windowtext;",
                "	border-left:none;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl159",
                "	{mso-style-parent:style113;",
                "	font-size:7.0pt;",
                "	font-weight:700;",
                "	font-style:italic;",
                "	text-align:center;",
                "	border-top:none;",
                "	border-right:none;",
                "	border-bottom:1.5pt solid windowtext;",
                "	border-left:none;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl160",
                "	{mso-style-parent:style126;",
                "	color:blue;",
                "	font-size:7.0pt;",
                "	font-weight:700;",
                "	text-decoration:underline;",
                "	text-underline-style:single;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl161",
                "	{mso-style-parent:style113;",
                "	font-size:14.0pt;",
                "	font-weight:700;",
                "	font-style:italic;",
                "	text-align:center;",
                "	background:white;",
                "	mso-pattern:black none;}",
                ".xl162",
                "	{mso-style-parent:style126;",
                "	color:blue;",
                "	font-size:7.0pt;",
                "	text-decoration:underline;",
                "	text-underline-style:single;",
                "	text-align:center;",
                "	background:white;",
                "	mso-pattern:black none;",
                "	white-space:normal;}",
                ".xl163",
                "	{mso-style-parent:style113;",
                "	font-size:12.0pt;",
                "	font-weight:700;",
                "	text-align:center;",
                "	background:white;",
                "	mso-pattern:black none;}" };
            #endregion

            string currentSheet = Constants.webpageDir + sheetName + @"_files\stylesheet.css";
			Common.DebugWriteLine(debug, "Web Debug.StoreSheets: " + currentSheet);

			using (FileStream fs = new FileStream(currentSheet, FileMode.Create))
			{
				using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
				{
					WriteToFile(cssString, w);
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
