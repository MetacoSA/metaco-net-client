using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class AssetsHistoryResult 
	{
		[JsonProperty("timestamp")]
		public DateTimeOffset Timestamp
		{
			get; 
			set;
		}

		[JsonProperty("assets")]
		public IEnumerable<AssetHistory> Assets
		{
			get; 
			set;
		}

	}
}