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
        static readonly bool debug = Constants.roomDebug;

        public Room()
        {
            RoomID = 1;
        }

        public Room(string rawRoom)
        {
            Regex roomPattern = new Regex(@"^(ASCB|ASCL|BIOS|ET|FA|HDFC|KH|LACHSA|MUS|PE|SH|ST|TA|TVFM)\s?([A-F]|LH)?(\d{1,4})([A-G])?");
            Building = roomPattern.Match(rawRoom).Groups[1].Value;
            Wing = roomPattern.Match(rawRoom).Groups[2].Value;
            int.TryParse(roomPattern.Match(rawRoom).Groups[3].Value, out int roomNumber);
            RoomNumber = roomNumber;
            SubRoom = roomPattern.Match(rawRoom).Groups[4].Value;

            string[] colname = new[] { "building", "wing", "roomNumber", "subRoom" };
            var coldata = new object[] { Building, Wing, RoomNumber, SubRoom };
            var tuple = Db.GetTuple("Room", "*", colname, coldata);
            
            bool noRoomInDb = tuple.Count == 0;
            if (noRoomInDb)
            {
                RoomID = Db.Insert("Room", colname, coldata);
            }
            else
            {
                RoomID = Convert.ToInt32(tuple[0].ToString());
            }
            Common.DebugWriteLine(debug, "Room.cs: RoomID = " + RoomID + " Building =" + Building + " Wing =" + Wing + " RoomNumber =" + RoomNumber + " SubRoom =" + SubRoom);
        }

    }
}
