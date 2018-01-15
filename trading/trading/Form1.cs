using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace trading
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            #region Declarations
            //------------------------------------------------------------------------------------------
            // String Declaration
            //------------------------------------------------------------------------------------------
            //string read_file_name = "C:\\Users\\selwart\\Documents\\test1.txt";
            //string write_file_name = "C:\\Users\\selwart\\Documents\\test1out.txt";
            string read_file_name = "\\test1.txt";
            read_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + read_file_name;
            // use in matlab with data file loaded: csvwrite('test1.txt',[lat; lon; GPS_Data_Nav_3_GPS_Heading']');
            //   in order to get data into format of lat, lon, heading or a, b, c;

            string write_file_name = "\\test1out.txt";
            write_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + write_file_name;
            string textL = "";
            //string textLCat = "";
            string textCat = "";
            //string textCat_abc = "";
            string textLCat_a = "";
            string textLCat_b = "";
            string textLCat_c = "";

            //------------------------------------------------------------------------------------------
            // Scalar Declaration
            //------------------------------------------------------------------------------------------
            //double _textL = 0;
            int lenTextL = 0;

            #endregion

            if (System.IO.File.Exists(read_file_name) == true)
            {
                #region Read Data from File
                //------------------------------------------------------------------------------------------
                // Read data from file
                //------------------------------------------------------------------------------------------
                System.IO.StreamReader objReader;
                objReader = new System.IO.StreamReader(read_file_name);

                System.IO.StreamReader objReaderLen;
                objReaderLen = new System.IO.StreamReader(read_file_name);

                #endregion

                #region Run time array and array size declarations
                //------------------------------------------------------------------------------------------
                // Array Declaration
                //------------------------------------------------------------------------------------------
                do
                {
                    // Find the number of elements in the data file
                    objReaderLen.ReadLine();
                    lenTextL = lenTextL + 1;

                } while (objReaderLen.Peek() != -1);
                objReaderLen.Close();

                int ctr = 0;

                string lineOfText = "a,b,c";
                string[] wordArray = lineOfText.Split(',');

                double[] _a;
                _a = new double[lenTextL];
                double[] _b;
                _b = new double[lenTextL];
                double[] _c;
                _c = new double[lenTextL];

                #endregion

                #region Store Data in an array for processing
                do
                {
                    textL = objReader.ReadLine();
                    //textLCat = textLCat + textL + "\r\n";

                    //------------------------------------------------------------------------------------------
                    // Split Line of text into array by commas from .CSV file
                    //------------------------------------------------------------------------------------------
                    lineOfText = textL;
                    wordArray = lineOfText.Split(',');
                    _a[ctr] = double.Parse(wordArray[0]);
                    _b[ctr] = double.Parse(wordArray[1]);
                    _c[ctr] = double.Parse(wordArray[2]);

                    textLCat_a = textLCat_a + wordArray[0] + "\r\n";
                    textLCat_b = textLCat_b + wordArray[1] + "\r\n";
                    textLCat_c = textLCat_c + wordArray[2] + "\r\n";

                    //------------------------------------------------------------------------------------------
                    // Write Splite data to an array
                    //------------------------------------------------------------------------------------------


                    //------------------------------------------------------------------------------------------
                    // Write data to an array
                    //------------------------------------------------------------------------------------------
                    //_text_L[ctr] = double.Parse(textL);
                    ctr = ctr + 1;

                } while (objReader.Peek() != -1);
                //txtInLat.Text = textLCat;
                txtIna.Text = textLCat_a;
                txtInb.Text = textLCat_b;
                txtInc.Text = textLCat_c;

                objReader.Close();
                #endregion

                #region Index each element of an array and begin processing data
                //------------------------------------------------------------------------------------------
                // Calculate at each index
                //------------------------------------------------------------------------------------------
                string n_text = "";
                double[] n;
                n = new double[_a.Length];
                int[] _x;
                _x = new int[_a.Length];

                for (int i = 0; i < lenTextL; i++) //_text_L.Length; i++)
                {

                    #region trading
                    //------------------------------------------------------------------------------------------
                    // traiding
                    //------------------------------------------------------------------------------------------

                    if (i == 0)
                    {
                        _x[i] = 1;
                    }
                    else
                    {
                        _x[i] = _x[i - 1]+1;
                    }

                    // Rolling average



                    // Day based Moving average
                    n[i] = _c[i];
                    
                    
                    n_text = n_text + _a[i].ToString() + "\r\n";
                    #endregion

                    //------------------------------------------------------------------------------------------
                    // Plot data from _a and _b for each index
                    //------------------------------------------------------------------------------------------
                    chart1.Series["Series1"].Points.AddXY(_x[i], _a[i]);
                    chart1.Series["Series2"].Points.AddXY(_x[i],n[i]);


                    //var minVal = n[0] - 10;
                    //var maxVal = n[i] + 10;
                    //chart1.ChartAreas["ChartArea1"].AxisY.Minimum = minVal;
                    //chart1.ChartAreas["ChartArea1"].AxisY.Maximum = maxVal;

                    //chart1.Series["Series2"].Points.AddXY(_a[i],_b[i]);
                }
                #endregion

                #region Write data from array to text box
                //------------------------------------------------------------------------------------------
                // Write data to text box
                //------------------------------------------------------------------------------------------

                System.IO.StreamWriter objWriter;
                objWriter = new System.IO.StreamWriter(write_file_name);

                objWriter.Write(n);
                objWriter.Close();

                txtOut1.Text = n_text;


                #endregion

                //chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                //chart1.Series["Series1"].Color = Color.Red;

                //chart1.Series["Series2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                //chart1.Series["Series2"].Color = Color.Blue;  

            }
            else
            {
                MessageBox.Show("No such file " + read_file_name);
            }
        }
    }
}
