using Newtonsoft.Json;

namespace MetacoClient.Contracts
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
