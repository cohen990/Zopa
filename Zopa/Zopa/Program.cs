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

			Console.WriteLine("Found {0} lenders", lenders.Count);
			Console.WriteLine("loanAmount: {0}", loanAmount);

			Console.ReadLine();
		}
	}
}
