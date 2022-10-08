using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EasyShopping
{
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
            Population();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        static string j = @"C:\Users\Jamil H.Nakhleh\Documents\EasyShoppingDb.mdf";
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+j.ToString()+";Integrated Security=True;Connect Timeout=30");
        User user = new User();
        Login login = new Login();
        DashBoard dashBoard = new DashBoard();


        private void Population()
        {
            Con.Open();
            string query = "select * from ProductTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Filter()
        {
            Con.Open();
            string query = "select * from ProductTbl where PCat = N'" + Filtercombobox.SelectedItem.ToString() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            if (TitleTxt.Text == "" || AuthorTxt.Text == "" || QuantityTxt.Text == "" || PriceTxt.Text == "" || CatCombobox.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "insert into ProductTbl values(N'"+TitleTxt.Text+ "','" + AuthorTxt.Text + "',N'" + CatCombobox.SelectedItem.ToString() + "','" + QuantityTxt.Text + "','" + PriceTxt.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Saved Successfully!");
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

        private void Filtercombobox_SelectionChangeCommitted(object sender, EventArgs e)
        {
               Filter();
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            Population();
            Filtercombobox.SelectedIndex = -1;
        }

        private void Filtercombobox_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        private void Filtercombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }
        private void Reset()
        {
            TitleTxt.Text = AuthorTxt.Text = PriceTxt.Text = QuantityTxt.Text = "";
            CatCombobox.SelectedIndex = -1;
            
        }
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }
        int Key = 0;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TitleTxt.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            AuthorTxt.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            CatCombobox.SelectedItem = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            QuantityTxt.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            PriceTxt.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();

            if (TitleTxt.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void Deletebtn_Click(object sender, EventArgs e)
        {
            if (Key==0)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "delete from ProductTbl where PId="+Key+"";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Deleted Successfully!");
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

        private void Editbtn_Click(object sender, EventArgs e)
        {
            if (TitleTxt.Text == "" || AuthorTxt.Text == "" || QuantityTxt.Text == "" || PriceTxt.Text == "" || CatCombobox.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();                                                                                     //,N'" +
                    string query = "Update ProductTbl set PTitle=N'"+TitleTxt.Text+"',PAuthor='"+AuthorTxt.Text+"',PCat=N'"+CatCombobox.SelectedItem.ToString()+"',PQty="+QuantityTxt.Text+",PPrice="+PriceTxt.Text+" where PId ="+Key+"";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Updated Successfully!");
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            user.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            dashBoard.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            login.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Product_Load(object sender, EventArgs e)
        {

        }
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void Product_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }
    }
}