using System;

namespace LabManagement

{
    internal class Lock
    {
        public int Id { get; set; }
        public int Cw1 { get; set; }
        public int Ccw { get; set; }
        public int Cw2 { get; set; }

        public Lock()
        {
        }


        public Lock(string lockNumber)
        {
            bool notNumeric = !int.TryParse(lockNumber, out int n);
            if (notNumeric)
            {
                Cw1 = -1;
                return;
            }

            //var returnedSQL = Db.GetTuple("Lock", "lockID", n.ToString());
            string[] colname = new[] { "lockID" };
            var coldata = new object[] { n };
            var tuple = Db.GetTuple("Lock", "*", colname, coldata);
            bool noLockInDb = tuple.Count == 0;


            if (noLockInDb)
            {
                Cw1 = -1;
                return;
            }

            Id = Convert.ToInt32(tuple[0].ToString());
            Cw1 = Convert.ToInt32(tuple[1].ToString());
            Ccw = Convert.ToInt32(tuple[2].ToString());
            Cw2 = Convert.ToInt32(tuple[3].ToString());
        }


        public Lock(int _number, int _cw1, int _ccw, int _cw2)
        {
            Id = _number;
            Cw1 = _cw1;
            Ccw = _ccw;
            Cw2 = _cw2;
        }
        public void SetLock(int _number, int _cw1, int _ccw, int _cw2)
        {
            Id = _number;
            Cw1 = _cw1;
            Ccw = _ccw;
            Cw2 = _cw2;
        }
        public static bool IsValidLockNumber(string lockNumber)
        {
            bool isNumeric = int.TryParse(lockNumber, out int n);
            bool isV30 = n > 0 && n < 201;
            bool isV652 = n > 600 && n < 801;
            bool ValidNumber = (isNumeric && (isV30 || isV652));
            return ValidNumber;
        }

    }
}