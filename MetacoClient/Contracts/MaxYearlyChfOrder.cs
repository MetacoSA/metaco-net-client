
using Newtonsoft.Json;

namespace Metaco.Client.Contracts
{
	public class MaxYearlyChfOrder 
	{
		[JsonProperty("remaining")]
		public long Remaining
		{
			get; 
			set;
		}
		[JsonProperty("current")]
		public long Current
		{
			get; 
			set;
		}
		[JsonProperty("max")]
		public long Max
		{
			get;
			set;
		}

	}
}
