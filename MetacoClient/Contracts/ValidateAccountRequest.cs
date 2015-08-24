using Newtonsoft.Json;

namespace Metaco.Client.Contracts
{
	public class ValidateAccountRequest 
	{
		[JsonProperty("code")]
		public string Code
		{
			get; 
			set;
		}
	}
}
