using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class Market
	{
		[JsonProperty("bid")]
		public decimal Bid
		{
			get; 
			set;
		}
		[JsonProperty("ask")]
		public decimal Ask
		{
			get; 
			set;
		}
		[JsonProperty("volume_daily")]
		public decimal VolumeDaily
		{
			get; 
			set;
		}
		[JsonProperty("yield_daily")]
		public decimal YieldDaily
		{
			get; 
			set;
		}
		[JsonProperty("yield_YTD")]
		public decimal YieldYTD
		{
			get; 
			set;
		}
		[JsonProperty("volatility_daily")]
		public decimal VolatilityDaily
		{
			get; 
			set;
		}
		[JsonProperty("issued")]
		public decimal Issued
		{
			get;
			set;
		}
	}
}
