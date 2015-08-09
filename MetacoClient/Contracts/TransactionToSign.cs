
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MetacoClient.Contracts
{
	public class TransactionToSign 
	{
		[JsonProperty("raw")]
		public string Raw
		{
			get;
			set;
		}

		[JsonProperty("inputs_to_sign")]
		public IEnumerable<InputsToSign> InputsToSign
		{
			get; 
			set;
		}

	}
}
