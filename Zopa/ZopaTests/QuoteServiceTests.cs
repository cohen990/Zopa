using System;
using System.Collections.Generic;
using NUnit.Framework;
using Zopa;

namespace ZopaTests
{
	[TestFixture]
	class QuoteServiceTests
	{
		private QuoteService QuoteService { get; set; }

		[SetUp]
		public void SetUp()
		{
			QuoteService = new QuoteService();
		}

		[Test]
		public void GivenNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => QuoteService.Get(null, 100));
		}

		[Test]
		public void GivenEmptyList_ReturnsQuoteUnavailable()
		{
			var result = QuoteService.Get(new List<Lender>(), 100);

			Assert.That(result.Available, Is.False);
		}

		[Test]
		public void GivenListOf1WithAdequateFunds_ReturnsQuoteAvailable()
		{
			List<Lender> lenders = new List<Lender> 
			{
				new Lender
				{
					AmountAvailable = 100,
					Name = "Lenny",
					Rate = 0.01
				}
			};

			var result = QuoteService.Get(lenders, 100);

			Assert.That(result.Available, Is.True);
		}
	}

	public class QuoteService
	{
		public Quote Get(List<Lender> lenders, decimal i)
		{
			if (lenders == null)
				throw new ArgumentNullException("lenders");

			if(lenders.Count == 0)
				return new Quote(false);

			return new Quote(true);
		}
	}

	public class Quote
	{
		public bool Available { get; private set; }

		public Quote(bool available)
		{
			Available = available;
		}
	}
}
