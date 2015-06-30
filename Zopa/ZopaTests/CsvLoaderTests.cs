using System;
using System.Collections.Generic;
using System.IO;
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

			Assert.Throws<ArgumentException>(() => Loader.Get(string.Empty));
		}

		[Test]
		public void GivenNoFileLocation_ThrowsArgumentException_WithUsefulErrorMessage()
		{
			var exception = Assert.Throws<ArgumentException>(() => Loader.Get(string.Empty));

			Assert.That(exception.Message, Is.EqualTo("Parameter 'csvLocation' cannot be empty"));
		}

		[Test]
		public void GivenWrongFileLocation_ThrowsFileNotFoundException()
		{
			Assert.Throws<FileNotFoundException>(() => Loader.Get("file.csv"));
		}

		[Test]
		public void GivenExistingFile_FindsFile()
		{
			var file = Loader.Get("market.csv");

			Assert.That(file, Is.Not.Null);
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
	}

	public class CsvLoader
	{
		public FileStream Get(string csvLocation)
		{
			if (string.IsNullOrWhiteSpace(csvLocation))
				throw new ArgumentException("Parameter 'csvLocation' cannot be empty", csvLocation);

			var file = File.Open(csvLocation, FileMode.Open, FileAccess.Read);

			return file;
		}

		public List<Lender> GetLenders(FileStream file)
		{
			throw new NotImplementedException();
		}
	}

	public class Lender
	{
		public string Name { get; set; }

		public double Rate { get; set; }

		public decimal Available { get; set; }
	}
}
