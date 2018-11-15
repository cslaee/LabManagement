namespace LabManagement

{
    internal class Lock
    {
        public int id;
        public int cw1;
        public int ccw;
        public int cw2;

        public Lock()
        {
        }


        public Lock(string lockNumber)
        {
            bool notNumeric = !int.TryParse(lockNumber, out int n);
            if (notNumeric)
            {
                cw1 = -1;
                return;
            }
            var returnedSQL = Db.GetId("Lock", n.ToString());
            if (returnedSQL.Count == 0)
            {
                cw1 = -1;
                return;
            }
            int.TryParse(returnedSQL[0], out id);
            int.TryParse(returnedSQL[1], out cw1);
            int.TryParse(returnedSQL[2], out ccw);
            int.TryParse(returnedSQL[3], out cw2);
        }
        public Lock(int _number, int _cw1, int _ccw, int _cw2)
        {
            id = _number;
            cw1 = _cw1;
            ccw = _ccw;
            cw2 = _cw2;
        }
        public void SetLock(int _number, int _cw1, int _ccw, int _cw2)
        {
            id = _number;
            cw1 = _cw1;
            ccw = _ccw;
            cw2 = _cw2;
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