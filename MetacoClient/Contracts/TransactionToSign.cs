using System.Collections.Generic;
using Newtonsoft.Json;

namespace Metaco.Client.Contracts
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
