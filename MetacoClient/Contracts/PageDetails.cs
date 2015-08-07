using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class PageDetails
	{
		[JsonProperty("number")]
		public int Number
		{
			get; 
			set;
		}
		[JsonProperty("size")]
		public int Size
		{
			get; 
			set;
		}
	}
}
