
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
		public Order[] Orders
		{
			get; 
			set;
		}
	}
}
