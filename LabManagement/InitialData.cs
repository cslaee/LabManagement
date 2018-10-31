using Newtonsoft.Json;
using System;
using System.IO;

namespace LabManagement
{
    class InitialData
    {
        static public int[,] lockCombo = new int[10, 4] {
             {1, 2, 3, 4}, {2, 3, 4, 5}, {3, 4, 5, 6}, {4, 5, 6, 7}, {5, 6, 7, 8}, {6, 7, 8, 9}, {7, 8, 9, 10}, {8, 9, 10, 11}, {9, 10, 11, 12}, {10, 11, 12, 13}
                        };

        //static public var[,] lockerType = new var[1, 4] {{1,1,1,1}};

        static public void Fill()
        {
            // Db.InsertRows("Lock", "id, cw1, ccw, cw2", lockCombo);
            Db.SaveArrayToJson(lockCombo);
            string locksFile = System.AppContext.BaseDirectory + Constants.locksJsonFileName;
            Console.WriteLine("dir =" + locksFile);
            Lock[] MasterLocks = JsonConvert.DeserializeObject<Lock[]>(File.ReadAllText(locksFile));
            Db.ObjToSql("Lock", "id, cw1, ccw, cw2", MasterLocks);
        }

    }
}
