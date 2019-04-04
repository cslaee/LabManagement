using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
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



    }
}

