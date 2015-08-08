using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class HistoryCriteria 
	{
		public HistoryCriteria(int from, int to, string freq, bool orderAsc)
		{
			this.From = from;
			this.To = to;
			this.Freq = freq;
			this.OrderAsc = orderAsc;
		}

		[JsonProperty("from")]
		public int From
		{
			get;
			set;
		}
		[JsonProperty("to")]
		public int To
		{
			get;
			set;
		}
		[JsonProperty("freq")]
		public string Freq
		{
			get;
			set;
		}
		[JsonProperty("orderAsc")]
		public bool OrderAsc
		{
			get;
			set;
		}

	}
}
