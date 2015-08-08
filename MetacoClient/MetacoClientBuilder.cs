using System;

namespace MetacoClient
{
	public class MetacoClientBuilder
	{
		public string MetacoApiId { get; private set; }
		public string MetacoApiKey { get; private set; }
		public string MetacoApiUrl { get; private set; }
		public bool MetacoTestingMode { get; private set; }

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
