using Newtonsoft.Json;

namespace Metaco.Client.Contracts
{
	public class AccountStatus {
		[JsonProperty("apiId")]
		public string ApiId
		{
			get; 
			set;
		}
		[JsonProperty("KYC1")]
		public bool Kyc1
		{
			get; 
			set;
		}
		[JsonProperty("KYC2")]
		public bool Kyc2
		{
			get; 
			set;
		}
		[JsonProperty("max_yearly_chf_order")]
		public MaxYearlyChfOrder MaxYearlyChfOrder
		{
			get; 
			set;
		}
		[JsonProperty("max_order_chf_value")]
		public long MaxOrderChfValue
		{
			get; 
			set;
		}
	}
}