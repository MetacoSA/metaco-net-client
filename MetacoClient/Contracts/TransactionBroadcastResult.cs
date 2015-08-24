using Newtonsoft.Json;

namespace Metaco.Client.Contracts
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
