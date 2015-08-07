using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class Definition 
	{
		[JsonProperty("ticker")]
		public string Ticker
		{
			get;
			set;
		}
		[JsonProperty("display")]
		public string Display
		{
			get;
			set;
		}
		[JsonProperty("contract")]
		public string Contract
		{
			get;
			set;
		}
		[JsonProperty("keywords")]
		public string Keywords
		{
			get;
			set;
		}
		[JsonProperty("unit")]
		public string Unit
		{
			get;
			set;
		}
		[JsonProperty("divisibility")]
		public int Divisibility
		{
			get;
			set;
		}
		[JsonProperty("asset_id")]
		public string AssetId
		{
			get;
			set;
		}
		[JsonProperty("issuer")]
		public Issuer Issuer
		{
			get;
			set;
		}
		[JsonProperty("kyc")]
		public Kyc Kyc
		{
			get;
			set;
		}
	}
}