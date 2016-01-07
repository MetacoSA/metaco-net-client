using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using JsonSerializer = Metaco.Client.JsonSerializer;
using System.Runtime.ExceptionServices;

namespace Metaco.Client
{
	internal class MetacoHttpClient
	{
		private readonly string _metacoApiId;
		private readonly string _metacoApiKey;
		private readonly Uri _metacoApiUrl;
		private readonly bool _metacoTestingMode;
		private readonly JsonSerializer _serializer;

		public MetacoHttpClient(string metacoApiId, string metacoApiKey, Uri metacoApiUrl, bool metacoTestingMode)
		{
			this._metacoApiId = metacoApiId;
			this._metacoApiKey = metacoApiKey;
			this._metacoApiUrl = metacoApiUrl;
			this._metacoTestingMode = metacoTestingMode;
			this._serializer = new JsonSerializer();
		}

		public string DebugInfo
		{
			get; 
			private set; 
		}

		private HttpClient CreateClient()
		{
			var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(2.0);
			if (!string.IsNullOrEmpty(_metacoApiId) && !string.IsNullOrEmpty(_metacoApiKey))
			{
				client.DefaultRequestHeaders.Add("X-Metaco-Id", _metacoApiId);
				client.DefaultRequestHeaders.Add("X-Metaco-Key", _metacoApiKey);
			}
			if (_metacoTestingMode)
			{
				client.DefaultRequestHeaders.Add("X-Metaco-Debug", "true");
			}

			return client;
		}

		public T Get<T>(string relativeAddress)
		{
			using(var client = CreateClient())
			{
                try
                {

                    var response = client.GetAsync(GetUrl(relativeAddress)).Result;
                    if(!response.IsSuccessStatusCode)
                        HandleInvalidResponse(response);

                    FetchDebugData(response);
                    var result = response.Content.ReadAsStringAsync().Result;

                    return _serializer.Deserialize<T>(result);
                }
                catch(AggregateException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    throw;
                }
			}
		}

		public void Delete(string relativeAddress)
		{
			using(var client = CreateClient())
			{
				var response = client.DeleteAsync(GetUrl(relativeAddress)).Result;
				if (!response.IsSuccessStatusCode)
					HandleInvalidResponse(response);
				FetchDebugData(response);
			}
		}

		public T Post<T, TR>(string relativeAddress, TR data)
		{
			using(var client = CreateClient())
			{
				var json = _serializer.Serialize(data);
				var content = new StringContent(json);
				if (!string.IsNullOrEmpty(json))
					content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var response = client.PostAsync(GetUrl(relativeAddress), content).Result;
				if (!response.IsSuccessStatusCode)
					HandleInvalidResponse(response);

				FetchDebugData(response);
				var result = response.Content.ReadAsStringAsync().Result;
				return _serializer.Deserialize<T>(result);
			}
		}

		public void Post<T>(string relativeAddress, T data)
		{
			using(var client = CreateClient())
			{
				var json = _serializer.Serialize(data);
				var content = new StringContent(json);
				if (!string.IsNullOrEmpty(json))
					content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var response = client.PostAsync(_metacoApiUrl + relativeAddress, content).Result;
				if (!response.IsSuccessStatusCode)
					HandleInvalidResponse(response);

				FetchDebugData(response);
			}
		}


		public static void HandleInvalidResponse(HttpResponseMessage response)
		{
			MetacoErrorResult metacoError;
			string content=null;
			Exception inner = null;
			try
			{
				content = response.Content.ReadAsStringAsync().Result;
				metacoError = JsonConvert.DeserializeObject<MetacoErrorResult>(content);
				if (string.IsNullOrEmpty(metacoError.MetacoError))
					throw new MetacoClientException(metacoError, ErrorType.UnknownError, content, (int) response.StatusCode, null);
			}
			catch (Exception e)
			{
				metacoError = new MetacoErrorResult {
					MetacoError = "", 
					Status = (int) response.StatusCode
				};
				inner = e;
			}
			var errorType = MetacoErrorsDefinitions.GetErrorType(metacoError);
			throw new MetacoClientException(metacoError, errorType, content, (int)response.StatusCode, inner);
		}

		private void FetchDebugData(HttpResponseMessage response)
		{
			var defaultInfo = default(KeyValuePair<string, IEnumerable<string>>);
			var debugInfo = response.Headers.FirstOrDefault(x => x.Key == "X-Metaco-DebugData");
			if(!debugInfo.Equals(defaultInfo))
				DebugInfo = string.Join(" || ", debugInfo.Value);
		}

		private string GetUrl(string relativeUrl)
		{
			return _metacoApiUrl + relativeUrl;
		}
	}
}
