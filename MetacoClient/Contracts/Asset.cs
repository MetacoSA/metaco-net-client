using Newtonsoft.Json;

namespace Metaco.Client.Contracts
{
	public class Asset 
	{
		[JsonProperty("definition")]
		public Definition Definition
		{
			get; 
			set;
		}
		[JsonProperty("underlying")]
		public string Underlying
		{
			get; 
			set;
		}
		[JsonProperty("market")]
		public Market Market
		{
			get; 
			set;
		}
	}
}

