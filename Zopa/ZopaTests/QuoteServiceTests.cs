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

		[Test]
		public void GivenListOf1WithAdequateFunds_ReturnsRateOf1Percent()
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

			Assert.That(result.Rate, Is.EqualTo(0.01));
		}

		[Test]
		public void GivenListOf2BothWithAdequateFunds_ReturnsLowerRate()
		{
			List<Lender> lenders = new List<Lender> 
			{
				new Lender
				{
					AmountAvailable = 100,
					Name = "Eddie",
					Rate = 0.05
				},
				new Lender
				{
					AmountAvailable = 100,
					Name = "Lenny",
					Rate = 0.02
				}
			};

			var result = QuoteService.Get(lenders, 100);

			Assert.That(result.Rate, Is.EqualTo(0.02));
		}

		[Test]
		public void GivenListOf2BothWithLessThanHalfFunds_ReturnsQuoteUnavailable()
		{
			List<Lender> lenders = new List<Lender> 
			{
				new Lender
				{
					AmountAvailable = 100,
					Name = "Eddie",
					Rate = 0.05
				},
				new Lender
				{
					AmountAvailable = 100,
					Name = "Lenny",
					Rate = 0.02
				}
			};

			var result = QuoteService.Get(lenders, 250);

			Assert.That(result.Available, Is.False);
		}

		[Test]
		public void GivenListOf2BothWithHalfFunds_ReturnsQuoteAvailable()
		{
			List<Lender> lenders = new List<Lender> 
			{
				new Lender
				{
					AmountAvailable = 100,
					Name = "Eddie",
					Rate = 0.05
				},
				new Lender
				{
					AmountAvailable = 100,
					Name = "Lenny",
					Rate = 0.02
				}
			};

			var result = QuoteService.Get(lenders, 200);

			Assert.That(result.Available, Is.True);
		}

		[Test]
		public void GivenListOf2BothWithHalfFunds_ReturnsAverageOfRates()
		{
			List<Lender> lenders = new List<Lender> 
			{
				new Lender
				{
					AmountAvailable = 100,
					Name = "Eddie",
					Rate = 0.05
				},
				new Lender
				{
					AmountAvailable = 100,
					Name = "Lenny",
					Rate = 0.02
				}
			};

			var result = QuoteService.Get(lenders, 200);

			Assert.That(result.Rate, Is.EqualTo(0.035));
		}

		[Test]
		public void GivenListOf2BothWithMoreThanHalfFunds_ReturnsCorrectWeightedRate()
		{
			List<Lender> lenders = new List<Lender> 
			{
				new Lender
				{
					AmountAvailable = 100,
					Name = "Eddie",
					Rate = 0.05
				},
				new Lender
				{
					AmountAvailable = 100,
					Name = "Lenny",
					Rate = 0.02
				}
			};

			var result = QuoteService.Get(lenders, 150);

			Assert.That(result.Rate, Is.EqualTo(0.03));
		}

		[Test]
		public void GivenManyLendersWithAdequateFunds_ReturnsCorrectRate()
		{
			List<Lender> lenders = new List<Lender> 
			{
				new Lender
				{
					AmountAvailable = 100,
					Name = "Eddie",
					Rate = 0.09
				},
				new Lender
				{
					AmountAvailable = 200,
					Name = "Lenny",
					Rate = 0.065
				},
				new Lender
				{
					AmountAvailable = 300,
					Name = "Lenny",
					Rate = 0.08
				},
				new Lender
				{
					AmountAvailable = 700,
					Name = "Lenny",
					Rate = 0.082
				},
				new Lender
				{
					AmountAvailable = 500,
					Name = "Lenny",
					Rate = 0.072
				}
			};

			var result = QuoteService.Get(lenders, 1000);

			Assert.That(result.Rate, Is.EqualTo(0.073));
		}
	}
}
