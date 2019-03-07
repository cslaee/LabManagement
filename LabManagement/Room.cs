using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LabManagement
{
    class Room
    {
        public int RoomID { get; set; }
        public string Building { get; set; }
        public string Wing { get; set; }
        public int RoomNumber { get; set; }
        public string SubRoom { get; set; }
        public string Name { get; set; }

        public Room(string rawRoom)
        {
            Regex roomPattern = new Regex(@"^(ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?([A-F]|LH)?(\d{1,4})([A-G])?\/?((ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?([A-F]|LH)?(\d{1,4})([A-G])?)?");
            int.TryParse(roomPattern.Match(rawRoom).Groups[3].Value, out int roomNumber);
            RoomNumber = roomNumber;
            Building = roomPattern.Match(rawRoom).Groups[1].Value;
            Wing = roomPattern.Match(rawRoom).Groups[2].Value;
            SubRoom = roomPattern.Match(rawRoom).Groups[4].Value;

            //    var userTuple = Db.GetTuple(this, "last = '" + Last + "'");
            //    bool noUserInDb = userTuple.Count == 0;
            //    if (noUserInDb)
            //    {
            //        UserType = 4;
            //        string insertColumns = "last, userTypeFK";
            //        string insertData = "'" + Last + "', '" + UserType + "'";
            //        UserID = Db.SqlInsert("User", insertColumns, insertData);
            //        Console.Write("Inserting User" + insertColumns + " " + insertData + "Returned UserID =" + UserID);
            //    }
        }





    }
}
