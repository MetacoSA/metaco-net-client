using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class Issuer 
	{
		[JsonProperty("name")]
		public string Name
		{
			get;
			set;
		}

		[JsonProperty("address")]
		public string Address
		{
			get;
			set;
		}

		[JsonProperty("contact")]
		public string Contact
		{
			get;
			set;
		}
	}
}
