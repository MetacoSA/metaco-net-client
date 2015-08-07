using System.Collections.Generic;
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class WalletDetails 
	{
		[JsonProperty("timestamp")]
		public int Timestamp
		{
			get; 
			set;
		}
		[JsonProperty("page")]
		public PageDetails PageDetails
		{
			get; 
			set;
		}
		[JsonProperty("addresses")]
		public List<string> Addresses
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
		[JsonProperty("balance_bitcoin")]
		public long BalanceBitcoin
		{
			get; 
			set;
		}
		[JsonProperty("balances")]
		public List<Balance> Balances
		{
			get; 
			set;
		}
		[JsonProperty("transactions")]
		public List<Transaction> Transactions
		{
			get;
			set;
		}
	}
}