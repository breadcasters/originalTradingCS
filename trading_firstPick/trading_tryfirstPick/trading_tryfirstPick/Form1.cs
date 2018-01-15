using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace trading_tryfirstPick
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

            Dictionary<int, string> dateFwd = new Dictionary<int, string>();
            Dictionary<int, double> openFwd = new Dictionary<int, double>();
            Dictionary<int, double> highFwd = new Dictionary<int, double>();
            Dictionary<int, double> lowFwd = new Dictionary<int, double>();
            Dictionary<int, double> closeFwd = new Dictionary<int, double>();
            Dictionary<int, double> volumeFwd = new Dictionary<int, double>();
            Dictionary<int, double> adjCloseFwd = new Dictionary<int, double>();

            Dictionary<int, string> date = new Dictionary<int, string>();
            Dictionary<int, double> open = new Dictionary<int, double>();
            Dictionary<int, double> high = new Dictionary<int, double>();
            Dictionary<int, double> low = new Dictionary<int, double>();
            Dictionary<int, double> close = new Dictionary<int, double>();
            Dictionary<int, double> volume = new Dictionary<int, double>();
            Dictionary<int, double> adjClose = new Dictionary<int, double>();

            //
            Dictionary<int, double> shortSMA = new Dictionary<int, double>();
            Dictionary<int, double> longSMA = new Dictionary<int, double>();

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
                dateFwd.Clear();
                openFwd.Clear();
                highFwd.Clear();
                lowFwd.Clear();
                closeFwd.Clear();
                volumeFwd.Clear();
                adjCloseFwd.Clear();
                date.Clear();
                open.Clear();
                high.Clear();
                low.Clear();
                close.Clear();
                volume.Clear();
                adjClose.Clear();
                //optSymbolName.Clear();
                optShortDay.Clear();
                optLongDay.Clear();
                optPctRtrn.Clear();

                #region Read data from file
                excSkip = false;
                try
                {

                    string read_file_name = "\\" + NYSEsymbol[iNYSE] + "testOut1_post1.csv";
                    read_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + read_file_name;
                    using (TextReader infile = File.OpenText(read_file_name))
                    {
                        string line;
                        int lineCtr = -19;
                        while ((line = infile.ReadLine()) != null)
                        {
                            lineCtr = lineCtr + 1;
                            if (lineCtr > 0)  // Ignore first line header
                            {
                                string[] parts = line.Split(',');
                                dateFwd.Add(dateFwd.Count, parts[0]);
                                openFwd.Add(openFwd.Count, Convert.ToDouble(parts[1]));
                                highFwd.Add(highFwd.Count, Convert.ToDouble(parts[2]));
                                lowFwd.Add(lowFwd.Count, Convert.ToDouble(parts[3]));
                                closeFwd.Add(closeFwd.Count, Convert.ToDouble(parts[4]));
                                volumeFwd.Add(volumeFwd.Count, Convert.ToDouble(parts[5]));
                                adjCloseFwd.Add(adjCloseFwd.Count, Convert.ToDouble(parts[6]));
                            }
                        }
                    }
                    read_file_name = "\\" + NYSEsymbol[iNYSE] + "_MaxOptimize_Out1.csv"; 
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
                                //optSymbolName.Add(optSymbolName.Count, parts[0]);
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

                if (excSkip == false)
                {
                    #region Read data from end to beginning
                    // this is how I always do it
                    for (int i = dateFwd.Count - 1; --i >= 0; )
                    {
                        // write date in reverse
                        date.Add(date.Count, dateFwd[i]);
                        open.Add(open.Count, openFwd[i]);
                        high.Add(high.Count, highFwd[i]);
                        low.Add(low.Count, lowFwd[i]);
                        close.Add(close.Count, closeFwd[i]);
                        volume.Add(volume.Count, volumeFwd[i]);
                        adjClose.Add(adjClose.Count, adjCloseFwd[i]);
                    }
                    #endregion Read data from end to beginning

                    #region Optimization
                    double pctRtrn = 0;
                    double buyTmp = 0;
                    bool bought = false;
                    bool sold = false;
                    string optiOutData = "";
                    // Optimization loop (loop through with new data to maximize the sell signal


                    for (int optDay = 0; optDay < optShortDay.Count; optDay++)
                    {
                        if (optPctRtrn[optDay] > 0 && close.Count>0) // ignore returns that are zero
                        {

                            pctRtrn = 0;
                            buyTmp = 0;
                            bought = false;
                            sold = false;

                            #region Calculate buy / sell signal
                            // Calculate buy sell signal using optimization values
                            //int shortDay = 10;
                            //int longDay = 20;
                            //int bullBearDay = 200;

                            double smaTmp = close[0];
                            double lmaTmp = close[0];
                            //double bbTmp = close[0];

                            // Calculate the short day moving average
                            // reset dictionaries at beginning of each loop
                            shortSMA.Clear();
                            int shortDay = Convert.ToInt32(optShortDay[optDay]);
                            int longDay = Convert.ToInt32(optLongDay[optDay]);
                            int ld;
                            for (int sd = 0; sd < close.Count; sd++)
                            {
                                if (sd + 1 > shortDay)
                                {
                                    smaTmp = 0;
                                    for (int j = 0; j < shortDay; j++)
                                    {
                                        smaTmp = smaTmp + close[sd - shortDay + j];
                                    }
                                    smaTmp = smaTmp / shortDay;
                                    shortSMA.Add(shortSMA.Count, smaTmp);

                                }
                                //}
                                // Calculate the long day moving average
                                // reset dictionaries at beginning of each loop
                                longSMA.Clear();

                                //for (ld = 0; ld < close.Count; ld++)
                                //{
                                ld = sd;
                                if (ld + 1 > longDay)
                                {
                                    lmaTmp = 0;
                                    for (int j = 0; j < longDay; j++)
                                    {
                                        lmaTmp = lmaTmp + close[ld - longDay + j];
                                    }
                                    lmaTmp = lmaTmp / longDay;
                                    longSMA.Add(longSMA.Count, lmaTmp);

                                }

                                int bs = ld;
                                if (bs < close.Count - 1)
                                {
                                    if (lmaTmp - smaTmp < 0 && (!bought)) // buy or hold long day is less than short day
                                    {
                                        bought = true;
                                        sold = false;
                                        buyTmp = close[bs + 1]; // add a buy date 1 day late to account for risk
                                    }
                                    else if (lmaTmp - smaTmp >= 0 && (bought)) // sell or wait long day is more than short day
                                    {
                                        bought = false;
                                        sold = true;
                                        // determine the cumulative percent return 
                                        // Add a risk threshold for to sell 1 day late
                                        pctRtrn = pctRtrn + (close[bs + 1] / buyTmp);
                                    }
                                }

                            }
                            #endregion Calculate buy / sell signal

                            #region write sma/lma and pct to file for each symbol
                            StreamWriter objWriter;
                            string write_file_name = "\\" + NYSEsymbol[iNYSE] + "_VerifyOptimize_Out1.csv";
                            write_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + write_file_name;
                            objWriter = new System.IO.StreamWriter(write_file_name);
                            optiOutData = optiOutData + NYSEsymbol[iNYSE] + "," + shortDay.ToString() + "," + longDay.ToString() + "," + pctRtrn.ToString() + "\r\n";
                            objWriter.Write(optiOutData);
                            objWriter.Close();
                            #endregion write sma/lma and pct to file for each symbol
                        }

                    #endregion Optimization
                    }
                }
                excSkip = false;
            }
        }
    }
}
