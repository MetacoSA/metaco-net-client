
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class TransactionBroadcastResult 
	{
		[JsonProperty("success")]
		public bool IsSuccess
		{
			get; 
			set;
		}
		[JsonProperty("error")]
		public BroadcastError Error
		{
			get;
			set;
		}
	}
}
