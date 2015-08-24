
using Newtonsoft.Json;

namespace Metaco.Client.Contracts
{
	public class NewTransaction 
	{
		[JsonProperty("from")]
		public string From
		{
			get; 
			set;
		}
		[JsonProperty("to")]
		public string To
		{
			get; 
			set;
		}
		[JsonProperty("change")]
		public string Change
		{
			get; 
			set;
		}
		[JsonProperty("ticker")]
		public string Ticker
		{
			get; 
			set;
		}
		[JsonProperty("amount_asset")]
		public int AmountAsset
		{
			get; 
			set;
		}
		[JsonProperty("amount_satoshi")]
		public int AmountSatoshi
		{
			get;
			set;
		}
		[JsonProperty("feePerKB")]
		public decimal FeePerKB
		{
			get; 
			set;
		}

	}
}
