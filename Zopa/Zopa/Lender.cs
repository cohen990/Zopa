using System.Diagnostics;

namespace Zopa
{
	[DebuggerDisplay("Lender: {Name}")]
	public class Lender
	{
		public string Name { get; set; }

		public double Rate { get; set; }

		public decimal AmountAvailable { get; set; }
	}
}