using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabManagement
{
    class EventType
    {
        public int EventTypeID { get; set; }
        public string Description { get; set; }

        public EventType(string name)
        {
            Description = name;
            Common.DebugWriteLine(true, "** New EventType with Description = " + Description);

            string[] colname = new[] { "description" };
            var coldata = new object[] { Description }; 
            var tuple = Db.GetTuple("EventType", "*", colname, coldata);
            bool coarseNotInDb = tuple.Count == 0;

            if (coarseNotInDb)
            {
                EventTypeID = Db.Insert("EventType", colname, coldata);
                Common.DebugWriteLine(true, "Description  " + Description + " with EventTypeID = " + EventTypeID + ", now inserted into Db");
            }
            else
            {
             //   EventTypeID = Convert.ToInt32(tuple[0].ToString());
                Common.DebugWriteLine(true, "Description " + Description + " with EventTypeID = " + EventTypeID + ", was in Db");
            }

        }
    }
}
