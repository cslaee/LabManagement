using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabManagement
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


                LabManagement.Db.StartDb();
            //Application.Run(new EmailCombinations());
            //ImportSchedule.GetExcelSchedule();

            ImportSchedule.TestImportSemesters();
            Web.PublishSchedule();
//            System.Environment.Exit(1);



            Application.Run(new Main());
        }
    }
}
