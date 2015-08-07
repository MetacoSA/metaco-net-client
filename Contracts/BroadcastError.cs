using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class BroadcastError 
	{
		[JsonProperty("code")]
		public string Code
		{
			get;
			set;
		}

		[JsonProperty("reason")]
		public string Reason
		{
			get; 
			set;
		}
	}
}
