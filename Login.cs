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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        AdminLogin adminLogin = new AdminLogin();
        Billing billing = new Billing();
        DataTable dt = new DataTable();
        SqlDataAdapter sda;
        static string j = @"C:\Users\Jamil H.Nakhleh\Documents\EasyShoppingDb.mdf";
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + j.ToString() + ";Integrated Security=True;Connect Timeout=30");
        public static string UserName = "";

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            LoginClass login = new LoginClass(UserNameTB.Text,PassTb.Text);
            Con.Open();
            sda = new SqlDataAdapter("Select Count(*) from UserTbl where UName='" + login.UserName + "' and UPass='"+login.Password+"' ",Con);
            sda.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                UserName = login.UserName;
                billing.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong UserName or Password!");
            }
            Con.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            adminLogin.Show();
            this.Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
