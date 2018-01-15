using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Mail;

// Program last - run every day to get buy / sell signal and send emil

namespace GetStockDataToday
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
            Dictionary<int, string> NYSEsymbol = new Dictionary<int, string>();
            Dictionary<int, int> LMA_Opt = new Dictionary<int, int>();
            Dictionary<int, int> SMA_Opt = new Dictionary<int, int>();
            Dictionary<int, double> Opt_retEst = new Dictionary<int, double>();

            //
            // Dictionaries
            Dictionary<int, string> NYSEname = new Dictionary<int, string>();
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
            Dictionary<int, bool> buyData = new Dictionary<int, bool>();
            

            Dictionary<int, string> optSymbolName = new Dictionary<int, string>();
            Dictionary<int, double> optShortDay = new Dictionary<int, double>();
            Dictionary<int, double> optLongDay = new Dictionary<int, double>();
            Dictionary<int, double> optPctRtrn = new Dictionary<int, double>();

            #endregion Initialization

            string write_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            write_file_name = write_file_name + "\\Trading\\Data";

            // Find stocks that are new

            // List of all companies on NYSE (from TopPicksOptimize_Out1.csv)
            #region Read data from
            string read_TopPicksNYSE = "\\TopPicksOptimize_Out1.csv";
            read_TopPicksNYSE = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Trading\\Data" + read_TopPicksNYSE;
            using (TextReader infile = File.OpenText(read_TopPicksNYSE))
            {
                string line;
                int lineCtr = 0;// -1;
                while ((line = infile.ReadLine()) != null)
                {
                    lineCtr = lineCtr + 1;
                    if (lineCtr > 0)  // Ignore first line header
                    {
                        string[] parts = line.Split(',');
                        NYSEsymbol.Add(NYSEsymbol.Count, parts[0]);
                        optLongDay.Add(optLongDay.Count, Convert.ToInt16(parts[1]));
                        optShortDay.Add(optShortDay.Count, Convert.ToInt16(parts[2]));
                        optPctRtrn.Add(optPctRtrn.Count, Convert.ToDouble(parts[3]));
                    }
                }
            }
            #endregion Read data from file
            // Loop through list of companies and store Historical data to csv file

            #region Check the day is a trading day
            //            	                    2013	    2014
            //New Years Day	                    January 1	January 1
            //Martin Luther King, Jr. Day	    January 21	January 20
            //Washington's Birthday	            February 18	February 17
            //Good Friday	                    March 29	April 18
            //Memorial Day	                    May 27	May 26
            //Independence Day	                July 4**	July 4**
            //Labor Day	                        September 2	September 1
            //Thanksgiving Day	                November 28*	November 27*
            //Christmas	                        December 25***	December 25***//

            #endregion Check the day is a trading day

            #region Get current Date
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();
            #endregion Get current Date

            #region Get first Date of quarter
            String dy_start = 1.ToString();
            String mn_start = "";
            String yy_start = "";
            if (Convert.ToInt16(mn) < 4) // first quarter
            {
                mn_start = 10.ToString();
                yy_start = (Convert.ToInt16(yy) - 1).ToString();
            }
            else if ((Convert.ToInt16(mn) < 7) && (Convert.ToInt16(mn) > 3))
            {
                mn_start = 1.ToString();
            }
            else if ((Convert.ToInt16(mn) < 10) && (Convert.ToInt16(mn) >6))
            {
                mn_start = 4.ToString();
            }
            else if (Convert.ToInt16(mn) > 9)
            {
                mn_start = 7.ToString();
            }

            #endregion Get first Date of quarter


            # region Get Stock or Index data from Yahoo
            // Reference Info:  http://code.google.com/p/yahoo-finance-managed/wiki/csvHistQuotesDownload
            // http://ichart.yahoo.com/table.csv?s=GOOG&a=0&b=1&c=2000

            // Base URL for Yohoo Finance
            string uriStart = "http://ichart.yahoo.com/table.csv?s=";
            // Stock or Index Id (may need to convert special characters: http://www.blooberry.com/indexdot/html/topics/urlencoding.htm)
            string uriID = "";// "AAPL";
            // From Date month (need to subtract one from the month), day, year
            int uriFromMonth = Convert.ToInt16(mn_start)-1;//9 - 1;
            int uriFromDay = Convert.ToInt16(dy_start);
            int uriFromYear = Convert.ToInt16(yy_start);
            string uriFromDate = "&a=" + uriFromMonth.ToString() + "&b=" + uriFromDay.ToString() + "&c=" + uriFromYear.ToString();
            // To Date month, day, year



            int uriToMonth = Convert.ToInt16(mn)-1;
            int uriToDay = Convert.ToInt16(dy);
            int uriToYear = Convert.ToInt16(yy);
            string uriToDate = "&d=" + uriToMonth.ToString() + "&e=" + uriToDay.ToString() + "&f=" + uriToYear.ToString();
            // Interval (day = d, week = 2, month = m)
            string uriIntervalType = "d";
            string uriInterval = "&g=" + uriIntervalType.ToString();
            // Static part of URL
            string uriStatic = "&ignore=.csv";
            // Complete URI

            string uri;
            for (int i = 0; i < NYSEsymbol.Count; i++)
            {
                //if (i == 0)
                //{
                //    uriID = "MOL";
                //}
                //else
                //{
                //    uriID = "GOOG";
                //}
                uriID = NYSEsymbol[i];
                uri = uriStart + uriID + uriFromDate + uriToDate + uriIntervalType + uriStatic;

                try
                {
                    getQuoteData(uri, uriID, write_file_name);
                }
                catch (Exception)
                {

                    //throw;
                }
            }



            //WebRequest GetURL = WebRequest.Create(uri);
            //Stream page = GetURL.GetResponse().GetResponseStream();
            //StreamReader sr = new StreamReader(page);

            //String csv = sr.ReadToEnd();

            //StreamWriter objWriter;
            //write_file_name = write_file_name + "\\" + uriID + "testOut1.csv";
            //objWriter = new System.IO.StreamWriter(write_file_name);
            //objWriter.Write(csv);
            //objWriter.Close();
            # endregion Get Stock or Index data from Yahoo

            // If file historical data already exists append with new data verify that new data does not match the latest data

            #region find if today is a buy or sell for each of the top selected stocks
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
                //optShortDay.Clear();
                //optLongDay.Clear();
                //optPctRtrn.Clear();

                #region Read data from file
                excSkip = false;
                try
                {

                    string read_file_name = "\\" + NYSEsymbol[iNYSE] + "testOut1_post1.csv";
                    read_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Trading\\Data" + read_file_name;
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

                    // don't run symbol if no data collected for that symbol
                    double noDataTmp = closeFwd[0];
                    //read_file_name = "\\" + NYSEsymbol[iNYSE] + "_MaxOptimize_Out1.csv";
                    //read_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Trading\\Data" + read_file_name;
                    //using (TextReader infile = File.OpenText(read_file_name))
                    //{
                    //    string line;
                    //    int lineCtr = -1;
                    //    while ((line = infile.ReadLine()) != null)
                    //    {
                    //        lineCtr = lineCtr + 1;
                    //        if (lineCtr > -1)  // Ignore first line header
                    //        {
                    //            string[] parts = line.Split(',');
                    //            //optSymbolName.Add(optSymbolName.Count, parts[0]);
                    //            optShortDay.Add(optShortDay.Count, Convert.ToDouble(parts[1]));
                    //            optLongDay.Add(optLongDay.Count, Convert.ToDouble(parts[2]));
                    //            optPctRtrn.Add(optPctRtrn.Count, Convert.ToDouble(parts[3]));
                    //        }
                    //    }
                    //}
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


                    //for (int optDay = 0; optDay < optShortDay.Count; optDay++)
                    //{
                        //if (optPctRtrn[optDay] > 0 && close.Count > 0) // ignore returns that are zero
                        //{

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
                            int shortDay = Convert.ToInt32(optShortDay[iNYSE]);
                            int longDay = Convert.ToInt32(optLongDay[iNYSE]);
                            int ld;
                            buyData.Clear();
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
                                buyData[sd] = bought;
                            }
                            #endregion Calculate buy / sell signal

                            #region write sma/lma and pct to file for each symbol
                            StreamWriter objWriter;
                            string write_file_name_bs = "\\" + NYSEsymbol[iNYSE] + uriToDate.ToString() + "_buysell.csv";
                            write_file_name_bs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Trading\\Data" + write_file_name_bs;
                            objWriter = new System.IO.StreamWriter(write_file_name_bs);
                            optiOutData = optiOutData + NYSEsymbol[iNYSE] + "," + bought.ToString() + "," + sold.ToString() + "\r\n";
                            objWriter.Write(optiOutData);
                            objWriter.Close();
                            #endregion write sma/lma and pct to file for each symbol
                    
                            #region email buy / sell signal for each symbol
                            var fromAddress = new MailAddress("breadcasters@gmail.com", "BreadCasting");
                            var toAddress = new MailAddress("shane@elwart.com", "Shane Elwart");
                            const string fromPassword = "Austin13";
                            string subject = "";
                            if (bought)
                            {
                                subject = "Ticker Symbol: " + NYSEsymbol[iNYSE] + ", BUY, " + uriToDate.ToString();
                            }
                            else
                            {
                                subject = "Ticker Symbol: " + NYSEsymbol[iNYSE] + ", SELL, " + uriToDate.ToString();
                            }
                            //string subject = NYSEsymbol[iNYSE];
                            string body = @"Investors should be cautious about any and all stock recommendations and 
                            should consider the source of any advice on stock selection. All investors are advised to
                            conduct their own independent research into individual stocks before making a purchase 
                            decision. In addition, investors are advised that past stock performance is no guarantee
                            of future price appreciation.  The information contained herein is proprietary to 
                            breadCasting.webs.com; may not be copied or distributed; and is not warranted to be accurate,
                            complete or timely. BreadCasting is not responsible for any damages or losses arising from 
                            any use of this information. Past performance is no guarantee of future results.";

                            var smtp = new SmtpClient
                            {
                                Host = "smtp.gmail.com",
                                Port = 587,
                                EnableSsl = true,
                                DeliveryMethod = SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = false,
                                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                            };
                            using (var message = new MailMessage(fromAddress, toAddress)
                            {
                                Subject = subject,
                                Body = body
                            })
                            {
                                smtp.Send(message);
                            }
                            #endregion email buy / sell signal for each symbol
                    //}

                    #endregion Optimization
                    //}
                }
                excSkip = false;
            }
            #endregion find if today is a buy or sell for each of the top selected stocks



        }

        private static void getQuoteData(string uri, string uriID, string write_file_name)
        {

            WebRequest GetURL = WebRequest.Create(uri);
            Stream page = GetURL.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(page);

            String csv = sr.ReadToEnd();

            StreamWriter objWriter;
            //write_file_name = write_file_name + "\\" + uriID + "testOut1.csv";
            write_file_name = write_file_name + "\\" + uriID + "testOut1_post1.csv";
            objWriter = new System.IO.StreamWriter(write_file_name);
            objWriter.Write(csv);
            objWriter.Close();
        }
    }
}
