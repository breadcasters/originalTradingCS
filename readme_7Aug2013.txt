% Steps for stock trading

0.) download list of stock symbols and store in ../My Documents/USA NYSE Listing.csv
1.) GetStockDataToday (every three months) need a set range of 1 year
2.) run trading_firstPick
option.) run GetStockDataToday (every three months) need a set range of 3 months
option.) run trading_tryfirstPick (every three months) 
option.) run trading_topPics (every three months)
3.) run GetStockDataToday (trading day only) one data added to data or from start of trading date to current date
4.)



================================

% Steps for stock trading

1.) download list of stock symbols and store in ../My Documents/USA NYSE Listing.csv
2.) Run GetStockData
	... set stock date range
	--> Outputs a list files with suffix testOut1.csv for each symbol in 1.) above in ../MyDocuments
3.) Run trading_firstPick
	... Set optimization ranges for short and long day
	--> Outputs a list of files with suffix _Optimize_Out1.csv for each symbol that includes symbol name, optimized short day, optimized long day, percent return over testOut1... files date range
4.) Run GetStockData
	... set stock date range for verification data (this should not be for the same date range used in step 1 above, but should follow the date range of step one)
	--> Outputs a list of files with suffix testOut1_post1.csv
5.) Run trading_tryfirstPick
	... uses _Optimize_Out1.csv files
	--> Outputs _VerifyOptimize_Out1.csv from _testOut1_post1.csv
6.) Run trading_topPickPreTrial
	... this is the pre-verified best picks (these should be used against the verified data to determine if both the verified and the optimized have similar returns)
	--> TopPicksPreTrialOptimize_Out.csv
7.) Run trading_topPics
	... this is just a check and really should be comparing verified with preverified