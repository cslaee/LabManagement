using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabManagement
{
    internal class User
    {
        public string userID;
        public string first;
        public string last;
        public string sid;
        public string email;
        public string phone;
        public string cell;
        public string userType;
        
        public User()
        {

        }
        public static string getColumnName(int colNumber)
        {
            switch (colNumber)
            {
                case 1:
                    return "first";
                case 2:
                    return "last";
                case 3:
                    return "email";
                case 4:
                    return "user_type";



                  
            }
                
            return " ";
        }        
    }
}
