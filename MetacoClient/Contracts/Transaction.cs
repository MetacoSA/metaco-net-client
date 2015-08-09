
using System;
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class Transaction 
	{
		[JsonProperty("created")]
		public DateTimeOffset Created
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
		[JsonProperty("type")]
		public string Type
		{
			get; 
			set;
		}
		[JsonProperty("confirmations")]
		public int Confirmations
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
		[JsonProperty("tx_hash")]
		public string TransactionHash
		{
			get; 
			set;
		}
		[JsonProperty("feePaid")]
		public long? FeePaid
		{
			get; 
			set;
		}
		[JsonProperty("order")]
		public Order Order
		{
			get;
			set;
		}

	}
}
