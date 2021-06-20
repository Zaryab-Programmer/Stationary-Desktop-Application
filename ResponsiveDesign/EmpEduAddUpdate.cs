﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResponsiveDesign
{
    public partial class EmpEduAddUpdate : Form
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-2UBR8R3;Initial Catalog=testsql;Integrated Security=True");
        SqlDataAdapter dataAdapter;

        EmpEducation empEducation;
        public EmpEduAddUpdate(EmpEducation empEducation)
        {
            InitializeComponent();

            this.empEducation = empEducation;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        [DllImport("user32.dll")]
        static extern bool AnimateWindow(IntPtr hWnd, int time, AnimateWindowFlags flags);

        [Flags]
        enum AnimateWindowFlags
        {
            AW_HOR_POSITIVE = 0x00000001,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_ACTIVATE = 0x00020000,
            AW_SLIDE = 0x00040000,
            AW_BLEND = 0x00080000
        }


        private void EmpEduAddUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnimateWindow(this.Handle, 1000, AnimateWindowFlags.AW_BLEND | AnimateWindowFlags.AW_HIDE);
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            String errorMsg;


            if (texBoxDegreeTitle.Text == "" && texBoxYear.Text == "" && texBoxInstitue.Text == "" )
            {
                errorMsg = "All field are Empty";
                errorMessage(errorMsg);
            }
            else if (texBoxDegreeTitle.Text == "")
            {
                errorMsg = "Degree is Empty. Please Fill Degree TextBox";
                errorMessage(errorMsg);
            }
            else if (texBoxYear.Text == "")
            {
                errorMsg = "Year is Empty. Please Fill Year TextBox";
                errorMessage(errorMsg);
            }
            else if (texBoxInstitue.Text == "")
            {
                errorMsg = "Institue is Empty. Please Fill Institue TextBox";
                errorMessage(errorMsg);
            }
            else
            {
                InsertData();

            }
        }
        private void InsertData()
        {
            try
            {
                string date = DateTime.Today.ToString("MM-dd-yyyy");
                string time = DateTime.Now.ToString("HH:mm:ss");

                int ID = getCompID();

                MessageBox.Show("Name: " + comboBox1.SelectedValue.ToString() + "Date : " + date + " Time : " + time + " ID : " + ID);



                String queryInsert = "insert into Emp_Education_table(FK_Emp_ID,DegreeTitle,Year,Institute,Date,Time) values('" + ID + "','" + texBoxDegreeTitle.Text + "','" + texBoxYear.Text + "', '" + texBoxInstitue.Text + "', '" + date + "','" + time + "')";
                con.Open();

                dataAdapter = new SqlDataAdapter();
                dataAdapter.InsertCommand = new SqlCommand(queryInsert, con);
                int i = dataAdapter.InsertCommand.ExecuteNonQuery();
                con.Close();

                if (i != 0)
                {
                    MessageBox.Show("Data Save");
                    empEducation.LoadRecord();
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Error" + ex);
            }
        }

        private int getCompID()
        {

            con.Open();
            String selectQuery = "Select Emp_ID from Emp_Profile_Table where Emp_Name ='" + comboBox1.SelectedValue.ToString() + "'";
            SqlCommand cmd = new SqlCommand(selectQuery, con);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return id;
        }

        private void errorMessage(string errorMsg)
        {
            MessageBox.Show(" " + errorMsg);
        }

        private void EmpEduAddUpdate_Load(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 500, AnimateWindowFlags.AW_BLEND);
            con.Open();
            String selectQuery = "Select Emp_Name from Emp_Profile_Table";
            SqlCommand cmd = new SqlCommand(selectQuery, con);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Emp_Name", typeof(string));
            dataTable.Load(reader);
            comboBox1.ValueMember = "Emp_Name";
            comboBox1.DataSource = dataTable;
            con.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void EmpEduAddUpdate_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
