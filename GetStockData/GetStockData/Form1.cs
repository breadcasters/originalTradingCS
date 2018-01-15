using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace GetStockData
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
            Dictionary<int, string> NYSEname = new Dictionary<int, string>();
            Dictionary<int, string> NYSEsymbol = new Dictionary<int, string>();
            Dictionary<int, string> NYSEcountry = new Dictionary<int, string>();

            //


            #endregion Initialization

            string write_file_name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            // Find stocks that are new
            
            // List of all companies on NYSE
            #region Read data from
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
                        NYSEsymbol.Add(NYSEsymbol.Count,parts[1]);
                        NYSEcountry.Add(NYSEcountry.Count, parts[2]);
                    }
                }
            }
            #endregion Read data from file
            // Loop through list of companies and store Historical data to csv file

            # region Get Stock or Index data from Yahoo
            // Reference Info:  http://code.google.com/p/yahoo-finance-managed/wiki/csvHistQuotesDownload
            // http://ichart.yahoo.com/table.csv?s=GOOG&a=0&b=1&c=2000

            // Base URL for Yohoo Finance
            string uriStart = "http://ichart.yahoo.com/table.csv?s=";
            // Stock or Index Id (may need to convert special characters: http://www.blooberry.com/indexdot/html/topics/urlencoding.htm)
            string uriID = "";// "AAPL";
            // From Date month (need to subtract one from the month), day, year
            int uriFromMonth = 9 - 1;
            int uriFromDay = 3;
            int uriFromYear = 2012;
            string uriFromDate = "&a=" + uriFromMonth.ToString() + "&b=" + uriFromDay.ToString() + "&c=" + uriFromYear.ToString();
            // To Date month, day, year

            

            int uriToMonth = 12 - 1;
            int uriToDay = 18;
            int uriToYear = 2012;
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

        private static void getQuoteData( string uri, string uriID, string write_file_name )
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
