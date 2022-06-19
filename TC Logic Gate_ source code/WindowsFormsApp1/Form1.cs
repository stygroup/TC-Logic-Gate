using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace TC_Logic_Gate
{
    public partial class Form1 : Form
    {
        private DataTable dataTable = new DataTable();
        private DataTable gridviewTable= new DataTable();
        private List<string> AFPList = new List<string>();
        private List<string> hCGList = new List<string>();
        private List<int> NoList = new List<int>();
        private List<double> AFPresultList = new List<double>();
        private List<double> HCGresultList = new List<double>();
        private List<string> FinalResultList = new List<string>();
        //Two thresholds
        public static double X1 = 10;
        public static double X2 = 0.01;

        public Form1()
        {
            InitializeComponent();
        }




        /// <summary>
        /// Click the button to trigger the calculation event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_compute_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < AFPList.Count; i++)
            {
                double y1 = Convert.ToDouble(AFPList[i]);
                double y2 = Convert.ToDouble(hCGList[i]);

                double x1 = Compute_x1(y1);
                double x2 = Compute_x2(y2);

                AFPresultList.Add(x1);
                HCGresultList.Add(x2);

                string res = service(x1, x2);
                FinalResultList.Add(res);
            }
            AddResult();
            gridviewTable = TransformDataTable(dataTable);
            BindDataForGridViewStyle(gridviewTable);
            
        }

        /// <summary>
        /// Calculating y1
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double Compute_x1(double y)
        {
            double res = 0;

            //You can modify the parameters here//
            //--------------------------------//
            double Y_max = 15625.12;
            double Y_bg = 1236.45;
            double s = 0.55664;
            double EC_50 = 13.1054;
            //--------------------------------//

            res = EC_50 * Math.Pow((Y_bg - y) / (y - Y_max), (1 / s));    //Equation of Calibration Curve AFP
            return res;
        }

        /// <summary>
        /// Calculating y2
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double Compute_x2(double y)
        {
            
            double res = 0;

            //You can modify the parameters here//
            //--------------------------------//
            double Y_max = 27132.92;
            double Y_bg = 2692.19;
            double s = 0.46323;
            double EC_50 = 1.52944;
            //--------------------------------//

            res = EC_50 * Math.Pow((Y_bg - y) / (y - Y_max), (1 / s));   //Equation of Calibration Curve hCG
            return res;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private string service(double x1, double x2)
        {
            string res = "";
            if(x1 < X1 && x2 < X2)
            
                res = "Safe."; 
                
            
            else
            
                res = "Alert!";
            
            return res;
        }

        /// <summary>
        /// Setting the y1 input box to only enter int. or decimals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox_y1_KeyPress(object sender, KeyPressEventArgs e)
        {          
            CheckNumber(sender, e);
        }

        /// <summary>
        /// Setting the y1 input box to only enter int. or decimals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox_y2_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumber(sender, e);
        }

        /// <summary>
        /// Checking if the input is an integer or decimal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckNumber(object sender, KeyPressEventArgs e)
        {
            
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != (char)('.') && e.KeyChar != (char)('-'))
            {
                 e.Handled = true;
            }
            if (e.KeyChar == (char)('-'))
            {
                if ((sender as TextBox).Text != "")
                {
                    e.Handled = true;
                }
            }
            
            if (e.KeyChar == (char)('.') && ((TextBox)sender).Text.IndexOf('.') != -1)
            {
                e.Handled = true;
            }
           
            if (e.KeyChar == (char)('.') && ((TextBox)sender).Text == "")
            {
                e.Handled = true;
            }
           
            if (e.KeyChar != (char)('.') && e.KeyChar != 8 && ((TextBox)sender).Text == "0")
            {
                e.Handled = true;
            }
            
            if (((TextBox)sender).Text == "-" && e.KeyChar == (char)('.'))
            {
                e.Handled = true;
            }

           // return false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Opening file 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Title = "Please select a .csv file";
            openFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                // textbox_x1.Text =  ReadCsv(filePath, 300);
                AFPList.Clear();
                hCGList.Clear();
                NoList.Clear();
                AFPList = ReadCsv(filePath,1);
                AddInputIntensity(AFPList,hCGList);
                gridviewTable = TransformDataTable(dataTable);
                BindDataForGridView(gridviewTable);
            }
        }
        /// <summary>
        /// Opening file 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if(AFPList.Count == 0)
            {
                MessageBox.Show("Please firstly input AFP then hCG.");
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Title = "Please select a .csv file";
            openFileDialog.Filter = "CSV file (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                hCGList.Clear();
                hCGList = ReadCsv(filePath,2);
                //textbox_x2.Text =  ReadCsv(filePath,500);
                AddInputIntensity(AFPList, hCGList);
                gridviewTable = TransformDataTable(dataTable);
                BindDataForGridView(gridviewTable);
            }
        }
        private List<string> ReadCsv(string filePath, int flag)
        {
            CsvStremReader csvStremReader = new CsvStremReader(filePath);
            int colnum = csvStremReader.ColCount;
            int rownum = csvStremReader.RowCount;
            if(colnum <= 0 || rownum <= 0)
            {
                MessageBox.Show("No data in the file!");
                return new List<string>();
            }
            CaculateHelper caculateHelper = new CaculateHelper();
            //List<List<double>> evendata = new List<List<double>>();//even column
            //List<List<double>> odddata = new List<List<double>>(); //odd column
            // Calculating the medians of even columns

            List<string> maxevendata = new List<string>();
            if(flag == 1)
            {
                for (int i = 1; i <= colnum / 2; i++)
                {
                    List<double> eventemplist = new List<double>();
                    List<double> evenmedialist = new List<double>();
                    for (int j = 2; j <= rownum; j++)
                    {
                        var temp = double.Parse(csvStremReader[j, i * 2-1]);
                        var temp1 = double.Parse(csvStremReader[j, i * 2]);
                        if ( temp >= 500 && temp <= 600)
                        {
                            eventemplist.Add(temp1);
                        }
                        evenmedialist.Add(temp1);

                    }
                    var median = caculateHelper.Median(evenmedialist.ToArray());
                    if (eventemplist.Count == 0)
                    {
                        maxevendata.Add("");
                    } else
                    {
                        var result = caculateHelper.GetMax(eventemplist,median);
                        maxevendata.Add(result.ToString());
                    }
                    NoList.Add(i);
                }
                
            } else
            {
                for (int i = 1; i <= colnum / 2; i++)
                {
                    List<double> eventemplist = new List<double>();
                    List<double> evenmedialist = new List<double>();

                    for (int j = 2; j <= rownum; j++)
                    {
                        var temp = double.Parse(csvStremReader[j, i * 2 - 1]);
                        var temp1 = double.Parse(csvStremReader[j, i * 2]);

                        if (temp >= 460 && temp <= 500)
                        {
                            eventemplist.Add(temp1);
                        }
                        evenmedialist.Add(temp1);
                    }
                    var median = caculateHelper.Median(evenmedialist.ToArray());

                    if (eventemplist.Count == 0)
                    {
                        maxevendata.Add("");
                    }
                    else
                    {
                        var result = caculateHelper.GetMax(eventemplist,median);
                        maxevendata.Add(result.ToString());
                    }
                }
            }
            return maxevendata;

            //var result = caculateHelper.GetMax(evendata[0]);
            //return result.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataTable.Columns.Add("Patient Number");
            dataTable.Columns.Add("Input of AFP Intensity");
            dataTable.Columns.Add("Output of AFP Concentration (ng/mL)");
            dataTable.Columns.Add("Input of hCG Intensity");
            dataTable.Columns.Add("Output of hCG Concentration (IU/mL)");
            dataTable.Columns.Add("Result");
           
        }
        private void BindDataForGridView(DataTable paramDt)
        {
            //int iColumnCount = paramDt.Columns.Count;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.Rows.Clear();
            dataGridView1.RowTemplate.Height = 55;
            dataGridView1.DataSource = paramDt;
        }

        private void BindDataForGridViewStyle(DataTable paramDt)
        {
            //int iColumnCount = paramDt.Columns.Count;
            dataGridView1.RowHeadersVisible = false;
            //dataGridView1.Rows.Clear();
            dataGridView1.RowTemplate.Height = 55;
            
            dataGridView1.DataSource = paramDt;
            for(int i = 0; i < dataGridView1.Columns.Count;i++)
            {
                if(dataGridView1.Rows[5].Cells[i].Value.ToString() != "Result")
                {
                    if(dataGridView1.Rows[5].Cells[i].Value.ToString() == "Alert!")
                    {
                        dataGridView1.Rows[5].Cells[i].Style.ForeColor = Color.Red;
                        dataGridView1.Rows[5].Cells[i].Style.BackColor = Color.Yellow;
                    } else if(dataGridView1.Rows[5].Cells[i].Value.ToString() == "Safe.")
                    {
                        dataGridView1.Rows[5].Cells[i].Style.ForeColor = Color.Green;
                    }
                }
            }
        }
        /// <summary>
        /// Transposing the table
        /// </summary>
        /// <param name="paramDt"></param>
        /// <returns></returns>
        private DataTable  TransformDataTable(DataTable paramDt)
        {
            DataTable dt = new DataTable();
            int iRowCount = paramDt.Rows.Count;
            int iColumnCount = paramDt.Columns.Count;
            //dt.Columns.Add("Head");
            for(int i = 1; i <= iRowCount; i++)
            {
                dt.Columns.Add("Data"+i.ToString());
            }
            for(int i = 0; i <= iColumnCount - 1; i++)
            {
                DataRow dr = dt.NewRow();
                //dr["Head"] = paramDt.Columns[i].ColumnName; 
                for(int j = 1; j <=iRowCount; j++)
                {
                    dr["Data" + j.ToString()] = paramDt.Rows[j-1][i].ToString();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void AddInputIntensity(List<string> intensitiesafp,List<string> intensitieshcg)
        {
            dataTable.Rows.Clear();
            for(int i = 0; i < intensitiesafp.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Input of AFP Intensity"] = Convert.ToDouble(intensitiesafp[i]).ToString("0.00");
                dataRow["Patient Number"] = NoList[i];
                dataTable.Rows.Add(dataRow);
            }
            for(int i = 0; i < intensitieshcg.Count; i++)
            {
                dataTable.Rows[i]["Input of hCG Intensity"] = Convert.ToDouble( intensitieshcg[i]).ToString("0.00");
            }
        }

        private void AddResult()
        {
            dataTable.Rows.Clear();
            for(int i = 0; i < AFPList.Count; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Patient Number"] = NoList[i];
                dataRow["Input of AFP Intensity"] = Convert.ToDouble(AFPList[i]).ToString("0.00");
                dataRow["Output of AFP Concentration (ng/mL)"] = string.Format("{0:0.####E+00}", Convert.ToDouble(AFPresultList[i]));
                dataRow["Input of hCG Intensity"] = Convert.ToDouble(hCGList[i]).ToString("0.00");
                dataRow["Output of hCG Concentration (IU/mL)"] = string.Format("{0:0.####E+00}",Convert.ToDouble(HCGresultList[i]));
                dataRow["Result"] = FinalResultList[i];
                dataTable.Rows.Add(dataRow);
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            dataGridView1.DataSource = dataTable;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xls files(*.xlsx)|*.xlsx|xls files(*.xls)|*.xls|All files(*.*)|*.*";
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.AddExtension = true;
            saveFileDialog.RestoreDirectory = true;
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = saveFileDialog.FileName;
                using(ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filepath)))
                {
                    if(package.Workbook.Worksheets.Count == 1)
                        package.Workbook.Worksheets.Delete(0);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Result");
                    worksheet.Cells[1, 1].Value = "Patient Number";
                    worksheet.Cells[1, 2].Value = "Input of AFP Intensity";
                    worksheet.Cells[1, 3].Value = "Output of AFP Concentration (ng/mL)";
                    worksheet.Cells[1, 4].Value = "Input of hCG Intensity";
                    worksheet.Cells[1, 5].Value = "Output of hCG Concentration (IU/mL)";
                    worksheet.Cells[1, 6].Value = "Result";
                    for(int i = 0; i < AFPList.Count; i++)
                    {
                        worksheet.Cells[2 + i, 1].Value = i + 1;
                        worksheet.Cells[2 + i, 2].Value = Convert.ToDouble(AFPList[i]).ToString("0.00");
                        worksheet.Cells[2 + i, 3].Value = string.Format("{0:0.####E+00}", Convert.ToDouble(AFPresultList[i]));
                        worksheet.Cells[2 + i, 4].Value = Convert.ToDouble(hCGList[i]).ToString("0.00");
                        worksheet.Cells[2 + i, 5].Value = string.Format("{0:0.####E+00}", Convert.ToDouble(HCGresultList[i]));
                        worksheet.Cells[2 + i, 6].Value = FinalResultList[i];
                    }
                    package.Save();
                }
                MessageBox.Show("Saved.");
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        
        private void button5_Click(object sender, EventArgs e)
        {
              this.WindowState = FormWindowState.Minimized;

        }
        private Point mPoint;
        

       


        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\v Information about the developers:\n   \tMing-Yu Guo\n   \tTianying Sun\n   \t(Sun Yat-sen University)\n\tEmail for questions of the software: guomy26@mail2.sysu.edu.cn\n\n\v", "About");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
