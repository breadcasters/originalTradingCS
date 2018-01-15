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

            // Integers

            // Double

            // Dictionaries
            Dictionary<int, string> NYSEsymbol = new Dictionary<int, string>();
            Dictionary<int, int> LMA_Opt = new Dictionary<int, int>();
            Dictionary<int, int> SMA_Opt = new Dictionary<int, int>();
            Dictionary<int, double> Opt_retEst = new Dictionary<int, double>();

            //


            #endregion Initialization

            string write_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            // Find stocks that are new

            // List of all companies on NYSE
            #region Read data from
            string read_TopPicksNYSE = "\\TopPicksOptimize_Out1.csv";
            read_TopPicksNYSE = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + read_TopPicksNYSE;
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
                        LMA_Opt.Add(LMA_Opt.Count, Convert.ToInt16(parts[1]));
                        SMA_Opt.Add(SMA_Opt.Count, Convert.ToInt16(parts[2]));
                        Opt_retEst.Add(Opt_retEst.Count, Convert.ToDouble(parts[3]));
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
            if (Convert.ToInt16(mn) < 4) // first quarter
            {
                mn_start = 1.ToString();
            }
            else if ((Convert.ToInt16(mn) < 7) && (Convert.ToInt16(mn) > 3))
            {
                mn_start = 4.ToString();
            }
            else if ((Convert.ToInt16(mn) < 10) && (Convert.ToInt16(mn) >6))
            {
                mn_start = 7.ToString();
            }
            else if (Convert.ToInt16(mn) > 9)
            {
                mn_start = 10.ToString();
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
            int uriFromYear = Convert.ToInt16(yy);
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
