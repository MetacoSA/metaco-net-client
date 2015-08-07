using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class Order 
	{
		[JsonProperty("id")]
		public string Id
		{
			get; 
			set;
		}
		[JsonProperty("created")]
		public int Created
		{
			get; 
			set;
		}
		[JsonProperty("expiration")]
		public int Expiration
		{
			get; 
			set;
		}
		[JsonProperty("status")]
		public string Status
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
		[JsonProperty("cancel_reason")]
		public string CancelReason
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
		[JsonProperty("amount_asset")]
		public long AmountAsset
		{
			get; 
			set;
		}
		[JsonProperty("user_has_signed")]
		public bool UserHasSigned
		{
			get; 
			set;
		}
		[JsonProperty("transaction")]
		public TransactionToSign Transaction
		{
			get;
			set;
		}
	}
}

