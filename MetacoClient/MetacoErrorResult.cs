using System;
using Newtonsoft.Json;

namespace Metaco.Client
{
	public class MetacoErrorResult
	{
		[JsonProperty("status")]
		public int Status { get; set; }
		[JsonProperty("metaco_error")]
		public String MetacoError { get; set; }
		[JsonProperty("location")]
		public String Location { get; set; }
		[JsonProperty("parameter_name")]
		public String ParameterName { get; set; }
		[JsonProperty("message")]
		public String Message { get; set; }
	}
}

