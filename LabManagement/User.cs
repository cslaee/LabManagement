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

        }

        public User(string newUser)
        {
            Last = newUser;
            var userTuple = Db.GetTuple(this, "last = '" + Last + "'");
            bool noUserInDb = userTuple.Count == 0;
            if (noUserInDb)
            {
                UserType = 4;
                string[] colname = new[] { "last", "userTypeFK" };
                var  coldata = new object[] { Last,  UserType  };
                Db.SqlInsert("User", colname, coldata); 
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
