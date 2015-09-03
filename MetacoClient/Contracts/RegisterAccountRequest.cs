using Newtonsoft.Json;

namespace Metaco.Client.Contracts
{
	public class RegisterAccountRequest 
	{
		[JsonProperty("phone")]
		public string Phone
		{
			get; 
			set;
		}

		[JsonProperty("provider_id")]
		public string ProviderId
		{
			get; 
			set;
		}
	}
}
