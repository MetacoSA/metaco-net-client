using System;
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class HistoryCriteria 
	{
		public HistoryCriteria(DateTimeOffset from, DateTimeOffset to, string freq, bool orderAsc)
		{
			this.From = from;
			this.To = to;
			this.Freq = freq;
			this.OrderAsc = orderAsc;
		}

		[JsonProperty("from")]
		public DateTimeOffset From
		{
			get;
			set;
		}
		[JsonProperty("to")]
		public DateTimeOffset To
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
