using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class HistoryCriteria 
	{
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
