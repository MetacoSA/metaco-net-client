using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class Kyc 
	{
		[JsonProperty("required")]
		public bool Required
		{
			get; 
			set;
		}
	}
}
