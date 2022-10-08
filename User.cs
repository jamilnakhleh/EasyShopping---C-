using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyShopping
{
    public partial class User : Form
    {
        public User()
        {
            InitializeComponent();
            Population();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        static string j = @"C:\Users\Jamil H.Nakhleh\Documents\EasyShoppingDb.mdf";
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + j.ToString() + ";Integrated Security=True;Connect Timeout=30");
        private void Population()
        {
            Con.Open();
            string query = "select * from UserTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            UserView.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Reset()
        {
            UserNameTB.Text = PhoneTb.Text = AddressTb.Text  = PassTb.Text = "";
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            if (UserNameTB.Text == "" || PhoneTb.Text == "" || AddressTb.Text == "" || PassTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "insert into UserTbl values('" + UserNameTB.Text + "','" + PhoneTb.Text + "',N'" + AddressTb.Text + "','" + PassTb.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Saved Successfully!");
                    Con.Close();
                    Population();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
        }

        private void Resetbtn_Click(object sender, EventArgs e)
        {
            Reset();
        }
        int Key = 0;
        private void Deletebtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "delete from UserTbl where UId=" + Key + "";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Deleted Successfully!");
                    Con.Close();
                    Population();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
        }

        private void UserView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UserView_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(UserView.SelectedRows[0].Cells[1].Value.ToString());
            UserNameTB.Text = UserView.SelectedRows[0].Cells[1].Value.ToString();
            PhoneTb.Text = UserView.SelectedRows[0].Cells[2].Value.ToString();
            AddressTb.Text = UserView.SelectedRows[0].Cells[3].Value.ToString();
            PassTb.Text = UserView.SelectedRows[0].Cells[4].Value.ToString();

            if (UserNameTB.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(UserView.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void Editbtn_Click(object sender, EventArgs e)
        {
            if (UserNameTB.Text == "" || PhoneTb.Text == "" || AddressTb.Text == "" || PassTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();                                                                                     //,N'" +
                    string query = "Update UserTbl set UName='"+UserNameTB.Text+"',UPhone="+PhoneTb.Text+",UAdd='"+AddressTb.Text+"',UPass='"+PassTb.Text+"' where UId ="+Key+"";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Updated Successfully!");
                    Con.Close();
                    Population();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Product login = new Product();
            login.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            DashBoard login = new DashBoard();
            login.Show();
            this.Hide();
        }
    }
}