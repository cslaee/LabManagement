using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SQLite;
using Newtonsoft.Json;

namespace LabManagement
{

    public partial class Main : Form
    {
        int userDataGrid_Row = 100;
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
            //activePanel.Visible = false;
            //activePanel = d; //I mean the Control, not an ID or something.
            //activePanel.Visible = true;
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
            SQLiteConnection connection = new SQLiteConnection(Constants.connectionString);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "select * from User";
            connection.Open();






            //conn.Open();
            //SQLiteCommand comm = new SQLiteCommand("Select * From Patients", conn);
            using (SQLiteDataReader read = command.ExecuteReader())
            {
                while (read.Read())
                {
                    userDataGrid.Rows.Add(new object[] {
            //read.GetValue(0),  // U can use column index
            read.GetValue(read.GetOrdinal("first")),  // Or column name like this
            read.GetValue(read.GetOrdinal("last")),
            read.GetValue(read.GetOrdinal("email"))
            });
                }
            }

        }








        private void dataGridView1_CancelRowEdit(object sender, QuestionEventArgs e)
        {

            Console.WriteLine("dataGridView1_CancelRowEdit");
        }






        private void DataGridView_CellEdit(object sender, EventArgs e)
        {
            Console.WriteLine("DataGridView_CellEdit this");
        }






        private void DataGridView_CellEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

            Console.WriteLine("DataGridView_CellEdit one");
        }

        
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string newCellValue = userDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
            string rowID = userDataGrid[0, e.RowIndex].Value.ToString();
            string colName = User.getColumnName(e.ColumnIndex);
            Console.WriteLine("dataGridView1_CellEndEdit two newCellValue = " + newCellValue + " Col =" + e.ColumnIndex + " Row =" +e.RowIndex);
            Db.UpdateID("User", "userID", rowID, colName, newCellValue);
//        userDataGrid[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Yellow;
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
            userDataGrid_RowStr = GetID(e.Row);
        }

        private string GetID(DataGridViewRow e)
        {
            string dataGridIndex = "-2";
            string sqliteIndex = "-1";

            if (e.Index > -1)
            {
                dataGridIndex = JsonConvert.SerializeObject(userDataGrid[0, e.Index].Value);
            }
           
            if (Int32.TryParse(dataGridIndex, out int itemIDint))
            {
                sqliteIndex = itemIDint.ToString();
            }
            Console.WriteLine("userDataGrid_RowStateChanged dataGridIndex = " + dataGridIndex + " sqliteIndex = " + sqliteIndex);
            return sqliteIndex;
        }

        private string GetIDString(DataGridViewRow e)
        {
            string dataGridIndex = "-2";
            string sqliteIndex = "-1";

            if (e.Index > -1)
            {
                dataGridIndex = JsonConvert.SerializeObject(userDataGrid[0, e.Index].Value);
            }
           
            if (Int32.TryParse(dataGridIndex, out int itemIDint))
            {
                sqliteIndex = itemIDint.ToString();
            }
            Console.WriteLine("userDataGrid_RowStateChanged dataGridIndex = " + dataGridIndex + " sqliteIndex = " + sqliteIndex);
            return sqliteIndex;
        }
    //for (int i = 0; i < dataGridView1.Rows[e.RowIndex].Cells.Count; i++)
    //{
    //    dataGridView1[i, e.RowIndex].Style.BackColor = Color.Yellow;
    //}


    }
}
