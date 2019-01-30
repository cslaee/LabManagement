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


            if (Constants.deleteDB == true)
            {
                LabManagement.ValidateDatabase.TestTables();
            }
            //Application.Run(new EmailCombinations());
            InitialData.ImportExcelData();
            Application.Run(new Main());
        }
    }
}
