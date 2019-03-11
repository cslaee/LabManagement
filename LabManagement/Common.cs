using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabManagement
{
    class Common
    {
        public static void DebugMessageCR(bool isDebug, string message)
        {
            if (isDebug)
                Console.WriteLine(message);
        }
         public static void DebugMessageNoCR(bool isDebug, string message)
        {
            if (isDebug)
                Console.Write(message);
        }
       
    }
}

