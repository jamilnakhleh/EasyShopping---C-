using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
 

namespace EasyShopping
{
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
            Population();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        static string j = @"C:\Users\Jamil H.Nakhleh\Documents\EasyShoppingDb.mdf";
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + j.ToString() + ";Integrated Security=True;Connect Timeout=30");
        private void Population()
        {
            Con.Open();
            string query = "select * from ProductTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProductDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        int newQty = 0;
        private void UpdateQuantity()
        {
              newQty = Stock - Convert.ToInt32(QuantityTxt.Text);
            try
            {
                Con.Open();                                                                                     //,N'" +
                string query = "Update ProductTbl set PQty=" + newQty + " where PId =" + Key + "";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Product Quantity Updated Successfully!");
                Con.Close();
                Population();
                //Reset();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void UpdateQuantityAfterRemove()
        {
            int rowIndex = -1;
            foreach (DataGridViewRow row in ProductDGV.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals(rmv_qty1))
                {
                    rowIndex = row.Index;
                    break;
                }
            }

            double newQty1 = Convert.ToDouble(ProductDGV.Rows[rowIndex].Cells[4].Value.ToString()) + rmv_num;
            MessageBox.Show(rmv_num.ToString());

            try
            {
                Con.Open();                                                                                     //,N'" +
                string query = "Update ProductTbl set PQty=" + newQty1 + " where PId =" + Convert.ToInt32(ProductDGV.Rows[rowIndex].Cells[0].Value.ToString()) + "";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Product Quantity Updated Successfully!");
                Con.Close();
                GrdTotal -= rmv_qty;
                TotalLBL.Text = "$" + Math.Round(GrdTotal,2);
                Population();
                //Reset();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        int n = 0;
        double total = 0, GrdTotal = 0;
        private void AddToBillBtn_Click(object sender, EventArgs e)
        {
            if (QuantityTxt.Text == "" || Stock  == 0 || Convert.ToInt32(QuantityTxt.Text) > Stock)
            {
                MessageBox.Show("Not Enough Stock!");
            }
            else
            {
                total = Convert.ToDouble(QuantityTxt.Text)* Convert.ToDouble(PriceTxt.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = TitleTxt.Text;
                newRow.Cells[2].Value = PriceTxt.Text;
                newRow.Cells[3].Value = QuantityTxt.Text;
                newRow.Cells[4].Value = Math.Round(total,2);
                BillDGV.Rows.Add(newRow);
                n++;
                GrdTotal += total;
                TotalLBL.Text = "$" + Math.Round(GrdTotal,2);
                UpdateQuantity();
            }
        }
        int Key = 0,Stock = 0;
        private void ProductDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TitleTxt.Text = ProductDGV.SelectedRows[0].Cells[1].Value.ToString();
           ClientNameTxt.Text = ProductDGV.SelectedRows[0].Cells[2].Value.ToString();
            //CatCombobox.SelectedItem = ProductDGV.SelectedRows[0].Cells[3].Value.ToString();
            QuantityTxt.Text = ProductDGV.SelectedRows[0].Cells[4].Value.ToString();
            PriceTxt.Text = ProductDGV.SelectedRows[0].Cells[5].Value.ToString();

            if (TitleTxt.Text == "")
            {
                Key = Stock = 0;
            }
            else
            {
                Key = Convert.ToInt32(ProductDGV.SelectedRows[0].Cells[0].Value.ToString());
                Stock = Convert.ToInt32(ProductDGV.SelectedRows[0].Cells[4].Value.ToString());
                
            }
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 600);

            if (ClientNameTxt.Text == "" || TitleTxt.Text == "")
            {
                MessageBox.Show("Select Client Name!");
            }
            else if (GrdTotal == 0)
            {
                MessageBox.Show("Add product to bill before printing the bill!");
            }
            else
            {
                try
                {
                    Con.Open();
                    foreach (DataGridViewRow row in BillDGV.Rows)
                    {
                        
                            prodid = Convert.ToInt32(row.Cells["Column1"].Value);
                            proName = "" + row.Cells["Column2"].Value;
                            prodprice = Convert.ToDouble(row.Cells["Column3"].Value);
                            prodqty = Convert.ToDouble(row.Cells["Column4"].Value);
                            total2 = Convert.ToDouble(row.Cells["Column5"].Value);
                        if (prodprice != 0)
                        {
                            string query = "insert into BillTbl values('" + ClientNameTxt.Text + "',N'" + proName + "'," + total2 + "," + GrdTotal + ", getdate())";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Bill Saved Successfully!");
                    Con.Close();
                    if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                    {
                        printDocument1.Print();
                    }
                    GrdTotal = 0;
                    TotalLBL.Text = "$" + Math.Round(GrdTotal,2);
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
        }
        int prodid, pos = 60;
        double prodqty, prodprice,total2;
        private void label4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        int Key1 = 0;
        double rmv_num = 0, rmv_qty = 0; 
        string rmv_qty1;
        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            Stock = Convert.ToInt32(ProductDGV.SelectedRows[0].Cells[4].Value.ToString());
            if (Key1 != 0)
            {
                try
                {
                    /* Con.Open();
                     string query = "delete from ProductTbl where PId=" + Key + "";
                     SqlCommand cmd = new SqlCommand(query, Con);
                     cmd.ExecuteNonQuery();
                     MessageBox.Show("Product Deleted Successfully!");
                     Con.Close();
                     Population();
                   */
                    foreach (DataGridViewCell oneCell in BillDGV.SelectedCells)
                    {
                        if (oneCell.Selected)
                        {
                            rmv_num = Convert.ToDouble(BillDGV.Rows[oneCell.RowIndex].Cells[3].Value);
                            rmv_qty = Convert.ToDouble(BillDGV.Rows[oneCell.RowIndex].Cells[4].Value);
                            rmv_qty1 = BillDGV.Rows[oneCell.RowIndex].Cells[1].Value.ToString();


                            UpdateQuantityAfterRemove();
                            BillDGV.Rows.RemoveAt(oneCell.RowIndex);
                        }
                    }
                   // Reset();
                    BillDGV.Refresh();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
            else MessageBox.Show("NO!");
        }

        private void BillDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (TitleTxt.Text == "")
            {
                Key1 = 0;
            }
            else
            {
                Key1 = Convert.ToInt32(BillDGV.SelectedRows[0].Cells[0].Value.ToString());
             
            }
        }
        private void ExportToExcel()
        {
            BillDGV.SelectAll();
            DataObject dataObj = BillDGV.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

        }
        private void ExporToXlsx_Click(object sender, EventArgs e)
        {
            //ExportToExcel();

            // creating Excel Application
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application  
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook  
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program  
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.  
            // store its reference to worksheet  
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            // changing the name of active sheet  
            worksheet.Name = "Exported from gridview";
            // storing header part in Excel  
            for (int i = 1; i < BillDGV.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = BillDGV.Columns[i - 1].HeaderText;
            }
            // storing Each row and column value to excel sheet  
            for (int i = 0; i < BillDGV.Rows.Count - 1; i++)
            {
                for (int j = 0; j < BillDGV.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = BillDGV.Rows[i].Cells[j].Value.ToString();
                }
            }
            // save the application  
           //workbook.SaveAs(@"C:\Debug\test1.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // Exit from the application  
            app.Quit();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            BillDetails billDetails = new BillDetails();
            billDetails.Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void Billing_Load(object sender, EventArgs e)
        {
            RemoveBtn.Visible = button1.Visible = false;
            UserName.Text = "Welcome Again, " + Login.UserName +" ! :)";
        }

        string proName;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("EasyShopping System", new Font("Century Gothic", 12, FontStyle.Bold),Brushes.Red,new Point(80));
            e.Graphics.DrawString("ID PRODUCT PRICE QUANTITY TOTAOL", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                prodid = Convert.ToInt32(row.Cells["Column1"].Value);
                proName = "" + row.Cells["Column2"].Value;
                prodprice = Convert.ToDouble(row.Cells["Column3"].Value);
                prodqty = Convert.ToDouble(row.Cells["Column4"].Value);
                total2 = Convert.ToDouble(row.Cells["Column5"].Value);
                e.Graphics.DrawString("" + prodid, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Red, new Point(26,pos));
                e.Graphics.DrawString("" + proName, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Red, new Point(45, pos));
                e.Graphics.DrawString("" + prodprice, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Red, new Point(120, pos));
                e.Graphics.DrawString("" + prodqty, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Red, new Point(170, pos));
                e.Graphics.DrawString("" + total2, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Red, new Point(235, pos));
             
                pos += 20;
            }
            e.Graphics.DrawString("Total : $" + Math.Round(GrdTotal,2), new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(60, pos + 50));
            e.Graphics.DrawString("**Easy Shopping System***", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(40, pos + 85));
            e.Graphics.DrawString(DateTime.Now.ToString(), new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(40, pos + 100));
            BillDGV.Rows.Clear();
            BillDGV.Refresh();
            pos = 100;
        }

        private void Reset()
        {
            TitleTxt.Text = ClientNameTxt.Text = PriceTxt.Text = QuantityTxt.Text = "";
        }
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}