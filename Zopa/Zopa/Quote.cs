namespace Zopa
{
	public class Quote
	{
		public bool Available { get; private set; }
		public double Rate { get; set; }

		private Quote(bool available, double rate)
		{
			Available = available;
			Rate = rate;
		}

		public Quote(double rate)
			: this(true, rate)
		{
		}

		public static Quote Unavailable()
		{
			return new Quote(false, 0);
		}
	}
}