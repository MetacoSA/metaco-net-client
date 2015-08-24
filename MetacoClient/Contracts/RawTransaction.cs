using Newtonsoft.Json;

namespace Metaco.Client.Contracts
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
