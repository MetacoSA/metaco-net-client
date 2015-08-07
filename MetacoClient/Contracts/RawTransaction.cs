
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class RawTransaction 
	{
		[JsonProperty("raw")]
		public string Raw
		{
			get; 
			set;
		}
	}
}
