using System;
using System.Linq;

namespace LabManagement
{
    internal class User
    {
        public int UserID { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Sid { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public int UserType { get; set; }

        public User()
        {
            UserID = 1;
        }

        public User(string newUser)
        {
            Last = newUser;

            string[] colname = new[] { "last" };
            var coldata = new object[] { Last };
            var tuple = Db.GetTuple("User", "*", colname, coldata);
            bool noUserInDb = tuple.Count == 0;
            if (noUserInDb)
            {
                UserType = 4;
                string[] colnameI = new[] { "last", "userTypeFK" };
                var coldataI = new object[] { Last, UserType };
                UserID = Db.Insert("User", colnameI, coldataI);
            }
            else
            {
                UserID = Convert.ToInt32(tuple[0].ToString());
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
