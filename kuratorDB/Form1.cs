using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace kuratorDB
{
    public partial class Form1 : Form
    {
        static string CONNECT_STR = string.Format("Provider = Microsoft.ACE.OLEDB.12.0; Data Source={0}",
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DUT_database.accdb"));

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DUT_database.accdb");
            //
            comboBox1.Items.Clear();
            OleDbConnection dbCon = new OleDbConnection(CONNECT_STR);
            dbCon.Open();
            DataTable tbls = dbCon.GetSchema("Tables", new string[] { null, null, null, "TABLE" }); //список всех таблиц
            foreach (DataRow row in tbls.Rows)
            {
                string TableName = row["TABLE_NAME"].ToString();
                comboBox1.Items.Add(TableName);
            }
            dbCon.Close();
            //
        }

        private void button1_Click(object sender, EventArgs e)
        {

            interfaceChangeOnLogin();
            
        }

        private void interfaceChangeOnLogin()
        {
            panel1.Visible = false;
            panel2.Visible = true;
            //panel2.Dock = DockStyle.Left;
            //panel3.Visible = true;
            //panel3.Dock = DockStyle.Bottom;
            dataGridView1.Visible = true;
            //dataGridView1.Dock = DockStyle.Fill;
        }

        private void interfaceChangeOnLogout()
        {
            panel1.Visible = true;
            panel2.Visible = false;
            //panel3.Visible = false;
            dataGridView1.Visible = false;
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            interfaceChangeOnLogout();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateView(comboBox1.Text);
        }

        private void UpdateView(string tableName)
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = GetTableByName(tableName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public static DataTable GetTableByName(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            string command = string.Format("SELECT * FROM {0}", tableName);
            using (OleDbConnection cnn = new OleDbConnection(CONNECT_STR))
            {
                cnn.Open();
                using (OleDbCommand cmd = new OleDbCommand(command, cnn))
                {
                    using (OleDbDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                }
            }
            return dt;
        }
    }
}
