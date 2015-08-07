using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class RegisterAccountRequest 
	{
		[JsonProperty("phone")]
		public string Phone
		{
			get; 
			set;
		}

	}
}
