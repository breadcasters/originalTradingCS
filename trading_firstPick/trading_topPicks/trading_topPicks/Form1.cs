using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace trading_topPicks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Initialization
            // Initialization -------------------------------------------------------------
            // Boolean
            bool excSkip = false;
            // Integers

            // Double

            // Dictionaries
            Dictionary<int, string> NYSEname = new Dictionary<int, string>();
            Dictionary<int, string> NYSEsymbol = new Dictionary<int, string>();
            Dictionary<int, string> NYSEcountry = new Dictionary<int, string>();

            Dictionary<int, string> optSymbolName = new Dictionary<int, string>();
            Dictionary<int, double> optShortDay = new Dictionary<int, double>();
            Dictionary<int, double> optLongDay = new Dictionary<int, double>();
            Dictionary<int, double> optPctRtrn = new Dictionary<int, double>();

            #endregion Initialization

            // List of all companies on NYSE
            #region Read data from List of all companies on NYSE
            string read_NYSElist = "\\USA NYSE Listing.csv";
            read_NYSElist = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + read_NYSElist;
            using (TextReader infile = File.OpenText(read_NYSElist))
            {
                string line;
                int lineCtr = -1;
                while ((line = infile.ReadLine()) != null)
                {
                    lineCtr = lineCtr + 1;
                    if (lineCtr > 0)  // Ignore first line header
                    {
                        string[] parts = line.Split(',');
                        NYSEname.Add(NYSEname.Count, parts[0]);
                        NYSEsymbol.Add(NYSEsymbol.Count, parts[1]);
                        NYSEcountry.Add(NYSEcountry.Count, parts[2]);
                    }
                }
            }
            #endregion Read data from List of all companies on NYSE

            // loop through all files from NYSE and use for optimization
            for (int iNYSE = 0; iNYSE < NYSEsymbol.Count; iNYSE++)
            {
                // Clear dictionaries for each new ticker symbol
                //optSymbolName.Clear();
                //optShortDay.Clear();
                //optLongDay.Clear();
                //optPctRtrn.Clear();

                #region Read data from file
                excSkip = false;
                try
                {
                    string read_file_name = "\\" + NYSEsymbol[iNYSE] + "_VerifyOptimize_Out1.csv";
                    read_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + read_file_name;
                    using (TextReader infile = File.OpenText(read_file_name))
                    {
                        string line;
                        int lineCtr = -1;
                        while ((line = infile.ReadLine()) != null)
                        {
                            lineCtr = lineCtr + 1;
                            if (lineCtr > -1)  // Ignore first line header
                            {
                                string[] parts = line.Split(',');
                                optSymbolName.Add(optSymbolName.Count, Convert.ToString(parts[0]));
                                optShortDay.Add(optShortDay.Count, Convert.ToDouble(parts[1]));
                                optLongDay.Add(optLongDay.Count, Convert.ToDouble(parts[2]));
                                optPctRtrn.Add(optPctRtrn.Count, Convert.ToDouble(parts[3]));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    excSkip = true;
                }
                #endregion Read data from file

            }


            //Sort
            Dictionary<int, double> sortOptPctRtn = optPctRtrn.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            Dictionary<int, double>.KeyCollection keys = sortOptPctRtn.Keys;

            #region write max return short day and long day
            StreamWriter objWriter;
            string maxoptiOutData = "";
            string write_file_name = "\\" + "TopPicksOptimize_Out1.csv";
            write_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + write_file_name;
            objWriter = new System.IO.StreamWriter(write_file_name);

            //string maxOptSymbolName;
            double maxOptShortDay;
            double maxOptLongDay;
            double maxOptPctRtrn;

            int ct = -1;
            int idx;
            foreach (KeyValuePair<int, double> pair in sortOptPctRtn)
            {
                ct = ct + 1;
                if (ct > Convert.ToInt32(sortOptPctRtn.Count) - 9)
                {
                    idx = pair.Key;

                    maxOptShortDay = optShortDay[idx];
                    maxOptLongDay = optLongDay[idx];
                    maxOptPctRtrn = sortOptPctRtn[idx];

                    maxoptiOutData = maxoptiOutData + optSymbolName[idx].ToString() + "," + maxOptShortDay.ToString() + "," + maxOptLongDay.ToString() + "," + maxOptPctRtrn.ToString() + "\r\n";
                }
            }

            objWriter.Write(maxoptiOutData);
            objWriter.Close();
            #endregion write max return short day and long day
        }
    }
}
