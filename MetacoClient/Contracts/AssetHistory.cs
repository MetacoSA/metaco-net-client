using System.Collections.Generic;
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class AssetHistory 
	{
		[JsonProperty("underlying")]
		public string Underlying
		{
			get; 
			set;
		}
		[JsonProperty("quotes")]
		public IEnumerable<decimal> Quotes 
		{
			get; 
			set;
		}
		[JsonProperty("times")]
		public IEnumerable<int> Times 
		{
			get; 
			set;
		}
		[JsonProperty("volumes")]
		public IEnumerable<decimal> Volumes
		{
			get; 
			set;
		}
	}
}