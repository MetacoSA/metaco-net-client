
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class AccountRegistrationResult {
		[JsonProperty("apiId")]
		public string ApiId { get; set; }
		[JsonProperty("apiKey")]
		public string ApiKey { get; set; }
	}
}
