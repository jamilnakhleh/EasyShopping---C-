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
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        static string j = @"C:\Users\Jamil H.Nakhleh\Documents\EasyShoppingDb.mdf";
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + j.ToString() + ";Integrated Security=True;Connect Timeout=30");

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
            User login = new User();
            login.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select count(PTitle) from ProductTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ProductLbl.Text = dt.Rows[0][0].ToString();

            //

            SqlDataAdapter sda1 = new SqlDataAdapter("Select sum(AmountTotal) from BillTbl", Con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            AmountLbl.Text = dt1.Rows[0][0].ToString();

            //

            SqlDataAdapter sda2 = new SqlDataAdapter("Select count(*) from UserTbl", Con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            UserLbl.Text = dt2.Rows[0][0].ToString();


            Con.Close();
        }

        private void ProductLbl_MouseHover(object sender, EventArgs e)
        {
           
        }
    }
}