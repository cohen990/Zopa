using System;
using System.Collections.Generic;
using System.Linq;

namespace Zopa
{
	public class QuoteService
	{
		public Quote Get(List<Lender> lenders, decimal requestedAmount)
		{
			if (lenders == null)
				throw new ArgumentNullException("lenders");

			if (lenders.Count == 0)
				return Quote.Unavailable();

			if (lenders.Sum(x => x.AmountAvailable) < requestedAmount)
				return Quote.Unavailable();

			lenders = lenders.OrderBy(x => x.Rate).ToList();

			decimal borrowed = 0;
			double weightedRate = 0;

			var i = 0;
			while (borrowed < requestedAmount)
			{
				var amountRemaining = requestedAmount - borrowed;

				var lender = lenders.ElementAt(i);


				if (amountRemaining < lender.AmountAvailable)
				{
					borrowed = requestedAmount;
					weightedRate += lender.Rate*(double) amountRemaining;
					break;
				}

				borrowed += lender.AmountAvailable;
				weightedRate += lender.Rate * (double) lender.AmountAvailable;

				i++;
			}

			return new Quote(weightedRate / (double) borrowed);
		}
	}
}