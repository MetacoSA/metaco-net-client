using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class AssetsHistoryResult 
	{
		[JsonProperty("timestamp")]
		public int Timestamp
		{
			get; 
			set;
		}

		[JsonProperty("assets")]
		public AssetHistory[] Assets
		{
			get; 
			set;
		}

	}
}