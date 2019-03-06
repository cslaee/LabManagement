using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabManagement
{
    internal class User
    {
        public int UserID;
        public string First;
        public string Last;
        public string Sid;
        public string Email;
        public string Phone;
        public string Cell;
        public int UserType;

        public User()
        {

        }

        public User(string newUser)
        {
            Last = newUser;
            var userTuple = Db.GetTuple(this, "last = '" + Last + "'");
            bool noUserInDb = userTuple.Count == 0;
            if (noUserInDb)
            {
                UserType = 4;
                string insertColumns = "last, userTypeFK";
                string insertData = "'" + Last + "', '" + UserType + "'";
                UserID = Db.SqlInsert("User", insertColumns, insertData);
                Console.Write("Inserting User" + insertColumns + " " + insertData + "Returned UserID =" + UserID);
            }
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
