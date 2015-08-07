using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class Balance 
	{
		[JsonProperty("ticker")]
		public string Ticker
		{
			get; 
			set;
		}

		[JsonProperty("amount")]
		public long Amount
		{
			get; 
			set;
		}

		[JsonProperty("value")]
		public long Value
		{
			get; 
			set;
		}
	}
}
