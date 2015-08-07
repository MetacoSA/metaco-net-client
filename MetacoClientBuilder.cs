using System;

namespace MetacoClient
{
	public class MetacoClientBuilder
	{
		public string MetacoApiId { get; set; }
		public string MetacoApiKey { get; set; }
		public string MetacoApiUrl { get; set; }
		public bool MetacoTestingMode { get; set; }

		public MetacoClient CreateClient()
		{
			return new MetacoClient(this);
		}

		public MetacoClientBuilder WithApiId(string apiId)
		{
			this.MetacoApiId = apiId;
			return this;
		}

		public MetacoClientBuilder WithApiKey(string apiKey)
		{
			this.MetacoApiKey = apiKey;
			return this;
		}

		public MetacoClientBuilder WithApiUrl(string apiUrl)
		{
			this.MetacoApiUrl = apiUrl;
			return this;
		}

		public MetacoClientBuilder WithTestingMode(bool testingMode)
		{
			this.MetacoTestingMode = testingMode;
			return this;
		}
	}
}
