using Newtonsoft.Json;

namespace Metaco.Client.Contracts
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
