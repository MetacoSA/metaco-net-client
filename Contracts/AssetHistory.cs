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
		public List<decimal> Quotes 
		{
			get; 
			set;
		}
		[JsonProperty("times")]
		public List<int> Times 
		{
			get; 
			set;
		}
		[JsonProperty("volumes")]
		public List<int> Volumes
		{
			get; 
			set;
		}
	}
}