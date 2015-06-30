using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace ZopaTests
{
	public class CsvLoaderTests
	{
		protected CsvLoader Loader { get; set; }

		[SetUp]
		public void SetUp()
		{
			Loader = new CsvLoader();
		}
	}

	[TestFixture]
	public class CsvLoaderGetTests : CsvLoaderTests
	{
		[Test]
		public void GivenNoFileLocation_ThrowsArgumentException()
		{

			Assert.Throws<ArgumentException>(() => Loader.GetAsString(string.Empty));
		}

		[Test]
		public void GivenNoFileLocation_ThrowsArgumentException_WithUsefulErrorMessage()
		{
			var exception = Assert.Throws<ArgumentException>(() => Loader.GetAsString(string.Empty));

			Assert.That(exception.Message, Is.EqualTo("Parameter 'csvLocation' cannot be empty"));
		}

		[Test]
		public void GivenWrongFileLocation_ThrowsFileNotFoundException()
		{
			Assert.Throws<FileNotFoundException>(() => Loader.GetAsString("file.csv"));
		}

		[Test]
		public void GivenExistingFile_FindsFile()
		{
			var content = Loader.GetAsString("market.csv");

			Assert.That(content, Is.Not.Null);
		}

		[Test]
		public void GivenCorrectFile_ResultContainsBob()
		{
			var content = Loader.GetAsString("market.csv");

			Assert.That(content, Is.StringContaining("Bob"));
		}
	}

	[TestFixture]
	public class CsvLoaderGetLendersTests : CsvLoaderTests
	{
		[Test]
		public void GivenNull_ReturnsEmptyList()
		{
			var result = Loader.GetLenders(null);

			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenSingleRow_ReturnsSingleLender()
		{
			var result = Loader.GetLenders("Bob,0.075,640");

			Assert.That(result.Count, Is.EqualTo(1));
		}

		[Test]
		public void GivenSingleRow_ReturnsCorrectLenderName()
		{
			var result = Loader.GetLenders("Bob,0.075,640");

			Assert.That(result.Single().Name, Is.EqualTo("Bob"));
		}

		[Test]
		public void GivenSingleRow_ReturnsCorrectLenderRate()
		{
			var result = Loader.GetLenders("Bob,0.075,640");

			Assert.That(result.Single().Rate, Is.EqualTo(0.075));
		}

		[Test]
		public void GivenSingleRow_ReturnsSingleLenderAmountAvailable()
		{
			var result = Loader.GetLenders("Bob,0.075,640");

			Assert.That(result.Single().AmountAvailable, Is.EqualTo(640));
		}

		[Test]
		public void GivenDifferentRow_ReturnsCorrectLenderName()
		{
			var result = Loader.GetLenders("Jane,0.069,480");

			Assert.That(result.Single().Name, Is.EqualTo("Jane"));
		}

		[Test]
		public void GivenDifferentRow_ReturnsCorrectLenderRate()
		{
			var result = Loader.GetLenders("Jane,0.069,480");

			Assert.That(result.Single().Rate, Is.EqualTo(0.069));
		}

		[Test]
		public void GivenDifferentRow_ReturnsSingleLenderAmountAvailable()
		{
			var result = Loader.GetLenders("Jane,0.069,480");

			Assert.That(result.Single().AmountAvailable, Is.EqualTo(480));
		}

		[Test]
		public void GivenTwoRows_ReturnsTwoLenders()
		{
			List<Lender> result = Loader.GetLenders("Bob,0.075,640\nJane,0.069,480");

			Assert.That(result.Count, Is.EqualTo(2));
		}

		[Test]
		public void GivenTwoRows_ReturnsBob()
		{
			List<Lender> result = Loader.GetLenders("Bob,0.075,640\nJane,0.069,480");

			Assert.That(result.First().Name, Is.EqualTo("Bob"));
		}

		[Test]
		public void GivenTwoRows_ReturnsJane()
		{
			List<Lender> result = Loader.GetLenders("Bob,0.075,640\nJane,0.069,480");

			Assert.That(result.Last().Name, Is.EqualTo("Jane"));
		}

		[Test]
		public void GivenOnlyHeaderRow_ReturnsNoLenders()
		{
			List<Lender> result = Loader.GetLenders("Lender,Rate,Available");

			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenFullFileContent_Returns7Lenders()
		{
			var content = File.ReadAllText("market.csv");

			List<Lender> result = Loader.GetLenders(content);

			Assert.That(result.Count, Is.EqualTo(7));
		}

		[Test]
		public void GivenFullFileContent_ReturnsDave()
		{
			var content = File.ReadAllText("market.csv");

			List<Lender> result = Loader.GetLenders(content);

			Assert.That(result.Count(x => x.Name == "Dave"), Is.EqualTo(1));
		}
	}

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
	}

	[DebuggerDisplay("Lender: {Name}")]
	public class Lender
	{
		public string Name { get; set; }

		public double Rate { get; set; }

		public decimal AmountAvailable { get; set; }
	}
}
