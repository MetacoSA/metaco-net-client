
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class OrderResultPage 
	{
		[JsonProperty("page")]
		public PageDetails PageDetails
		{
			get; 
			set;
		}
		[JsonProperty("orders")]
		public IEnumerable<Order> Orders
		{
			get; 
			set;
		}
	}
}
