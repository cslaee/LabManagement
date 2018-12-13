using Newtonsoft.Json;
using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace LabManagement
{

    public partial class Main : Form
    {
        string userDataGrid_RowStr;

        public Main()
        {
            InitializeComponent();
            PopulateUserDataGridView();
        }


        
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }
        private void classesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void partsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void mailCombinationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void Main_Load(object sender, EventArgs e)
        {
        }
        private void button3_Click(object sender, EventArgs e)
        {
        }
        private void Lockers_Click(object sender, EventArgs e)
        {
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }
        private void tabPage1_Click_1(object sender, EventArgs e)
        {
        }
        private void Configuration_Click(object sender, EventArgs e)
        {
        }
        private void AddCombo_Click(object sender, EventArgs e)
        {
        }
        private void Clear_Click(object sender, EventArgs e)
        {
        }
        private void send_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            EmailCombinations f2 = new EmailCombinations();
            f2.ShowDialog(); // Shows Form2
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void dataGridView1_RowEnter(object sender,
            DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < userDataGrid.Rows[e.RowIndex].Cells.Count; i++)
            {
                userDataGrid[i, e.RowIndex].Style.BackColor = Color.Yellow;
            }
        }

        private void dataGridView1_RowLeave(object sender,
            DataGridViewCellEventArgs e)
        {
            //for (int i = 0; i < dataGridView1.Rows[e.RowIndex].Cells.Count; i++)
            //{
            //    dataGridView1[i, e.RowIndex].Style.BackColor = Color.Empty;
            //}
        }





        private void PopulateUserDataGridView()
        {
            SQLiteConnection connection = new SQLiteConnection(Constants.connectionString);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "select * from User";
            connection.Open();
            //SQLiteCommand comm = new SQLiteCommand("Select * From Patients", conn);
            using (SQLiteDataReader read = command.ExecuteReader())
            {
                while (read.Read())
                {
                    userDataGrid.Rows.Add(new object[] {
            read.GetValue(0),  // U can use column index
            read.GetValue(read.GetOrdinal("first")),  // Or column name like this
            read.GetValue(read.GetOrdinal("last")),
            read.GetValue(read.GetOrdinal("email"))
            });
                }
            }

        }


        private void Add_Click(object sender, EventArgs e)
        {
        }
        private void dataGridView1_CancelRowEdit(object sender, QuestionEventArgs e)
        {
        }
        private void DataGridView_CellEdit(object sender, EventArgs e)
        {
        }
        private void DataGridView_CellEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string newCellValue = userDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
            string rowID = GetID(e.RowIndex);
            string colName = User.getColumnName(e.ColumnIndex);

            Console.WriteLine("|" + rowID + "|");
            if (rowID.Equals("-1"))
            {
                //Console.WriteLine(" return = " + Db.InsertRow("User", "'" + colName + "'", "'" + newCellValue + "'"));
                int rowIDme = Db.InsertRow("User", "'" + colName + "'", "'" + newCellValue + "'");
                userDataGrid[0, e.RowIndex].Value = rowIDme;
                //Db.InsertRow("User", "first, last, email", "'mike','obermeyer','m@o.com'");
                //addRow(colName, newCellValue);
                return;
            }

            Console.WriteLine("dataGridView1_CellEndEdit two newCellValue = " + newCellValue + " Col =" + e.ColumnIndex + " Row =" + e.RowIndex);
            Db.UpdateID("User", "userID", rowID, colName, newCellValue);
            //        userDataGrid[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Yellow;
        }

        private void addRow(string colName, string cellValue)
        {
            Db.InsertRow("User", colName, cellValue);
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Db.DeleteId("User", "userID", userDataGrid_RowStr);
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

            Console.WriteLine("dataGridView1_RowsAdded Four");
        }

        private void userDataGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            //Console.WriteLine("DataGridViewRowPostPaintEventArgs RoxIndex"+ e.RowIndex  + " e = " + userDataGrid[1, e.RowIndex].ToString());
        }

        private void userDataGrid_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            userDataGrid_RowStr = GetID(e.Row.Index);
        }

        private string GetID(int indx)
        {
            string sqliteIndex = "-1";
            if (indx > -1)
            {
                if (Int32.TryParse(JsonConvert.SerializeObject(userDataGrid[0, indx].Value), out int itemIDint))
                {
                    sqliteIndex = itemIDint.ToString();
                }
            }
            Console.WriteLine("userDataGrid_RowStateChanged sqliteIndex = " + sqliteIndex + " indx = " + indx);
            return sqliteIndex;
        }


        //for (int i = 0; i < dataGridView1.Rows[e.RowIndex].Cells.Count; i++)
        //{
        //    dataGridView1[i, e.RowIndex].Style.BackColor = Color.Yellow;
        //}


    }
}
