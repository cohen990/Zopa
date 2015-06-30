using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Zopa
{
	public class CsvLoader
	{
		public string GetAsString(string csvLocation)
		{
			if (string.IsNullOrWhiteSpace(csvLocation))
				throw new ArgumentException("Parameter 'csvLocation' cannot be empty", csvLocation);

			var content = File.ReadAllText(csvLocation);

			return content;
		}

		public List<Lender> GetLenders(string commaSeparatedLenders)
		{
			if (string.IsNullOrWhiteSpace(commaSeparatedLenders))
				return new List<Lender>();

			var lenders = new List<Lender>();

			var rows = commaSeparatedLenders.Split('\n');

			rows = rows.Where(x => !x.StartsWith("Lender,Rate,Available") && !string.IsNullOrWhiteSpace(x)).ToArray();

			foreach (var row in rows)
			{
				var columns = row.Split(',');

				var name = columns[0];
				var rate = double.Parse(columns[1]);
				var amountAvailable = decimal.Parse(columns[2]);
				
				lenders.Add(new Lender
				{
					Name = name,
					Rate = rate,
					AmountAvailable = amountAvailable
				});
			}

			return lenders;
		}

		public List<Lender> GetLendersFromFile(string fileLocation)
		{
			var fileContent = GetAsString(fileLocation);

			return GetLenders(fileContent);
		}
	}
}