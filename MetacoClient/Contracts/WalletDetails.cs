using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class WalletDetails 
	{
		[JsonProperty("timestamp")]
		public DateTimeOffset Timestamp
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
		public IEnumerable<string> Addresses
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
		public IEnumerable<Balance> Balances
		{
			get; 
			set;
		}
		[JsonProperty("transactions")]
		public IEnumerable<Transaction> Transactions
		{
			get;
			set;
		}
	}
}