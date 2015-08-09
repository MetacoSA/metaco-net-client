using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MetacoClient.Contracts
{
	public enum OrderType
	{
		Buy, Sell
	}

	public class NewOrder 
	{
		[JsonProperty("type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public OrderType Type
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
		public long AmountAsset
		{
			get; 
			set;
		}
		[JsonProperty("amount_satoshi")]
		public long AmountSatoshi
		{
			get; 
			set;
		}
		[JsonProperty("recipient")]
		public string Recipient
		{
			get; 
			set;
		}
		[JsonProperty("funding")]
		public IEnumerable<string> Funding 
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
		[JsonProperty("webhook")]
		public string Webhook
		{
			get;
			set;
		}
	}
}
