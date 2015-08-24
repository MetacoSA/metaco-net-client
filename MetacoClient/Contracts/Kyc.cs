using Newtonsoft.Json;

namespace Metaco.Client.Contracts
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
