using System;
using System.Collections.Generic;
using System.IO;

namespace Zopa
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.WriteLine("This program accepts 2 parameters - market_file <string> and loan_amount <decimal>");
				Console.WriteLine("Press any key to exit...");
				Console.ReadLine();
				return;
			}

			var marketFileLocation = args[0];

			decimal loanAmount;
			var isDecimal = decimal.TryParse(args[1], out loanAmount);

			if (!isDecimal)
			{
				Console.WriteLine("Unable to parse a decimal from your second parameter '{0}'", args[1]);

				Console.WriteLine("Press any key to exit...");
				Console.ReadLine();
				return;
			}

			var lenders = new List<Lender>();
			try
			{
				lenders = new CsvLoader().GetLendersFromFile(marketFileLocation);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("Unable to find the file '{0}'", marketFileLocation);

				Console.WriteLine("Press any key to exit...");
				Console.ReadLine();
				return;
			}

			var quote = new QuoteService().Get(lenders, loanAmount);

			if (!quote.Available)
			{
				Console.WriteLine("Unfortunately, we do not currently have the funds to loan to you. Please try a smaller amount.");

				Console.WriteLine("Press any key to exit...");
				Console.ReadLine();
				return;
			}

			/*
				Requested amount: £XXXX
				Rate: X.X%
				Monthly repayment: £XXXX.XX
				Total repayment: £XXXX.XX
			*/

			Console.WriteLine("Requested amount: {0}", loanAmount.ToString("C0"));
			Console.WriteLine("Rate: {0}", quote.Rate.ToString("P1"));
			// Do Repayment service next
			//Console.WriteLine("Monthly repayment: {0}", quote.Rate.ToString("P1"));

			Console.ReadLine();
		}
	}
}
