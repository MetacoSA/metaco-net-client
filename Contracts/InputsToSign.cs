using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class InputsToSign 
	{
		[JsonProperty("index")]
		public int Index
		{
			get;
			set;
		}
		[JsonProperty("signing_address")]
		public string SigningAddress
		{
			get;
			set;
		}
	}
}
