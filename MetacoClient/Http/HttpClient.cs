using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MetacoClient.Http
{
	public class MetacoHttpClient
	{
		private readonly string _metacoApiId;
		private readonly string _metacoApiKey;
		private readonly string _metacoApiUrl;
		private readonly bool _metacoTestingMode;

		public MetacoHttpClient(string metacoApiId, string metacoApiKey, string metacoApiUrl, bool metacoTestingMode)
		{
			this._metacoApiId = metacoApiId;
			this._metacoApiKey = metacoApiKey;
			this._metacoApiUrl = metacoApiUrl;
			this._metacoTestingMode = metacoTestingMode;
		}

		public string DebugInfo
		{
			get; 
			private set; 
		}

		private HttpClient CreateClient()
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Add("X-Metaco-Id", _metacoApiId);
			client.DefaultRequestHeaders.Add("X-Metaco-Key", _metacoApiKey);
			if (_metacoTestingMode)
				client.DefaultRequestHeaders.Add("X-Metaco-Debug", "true");
			return client;
		}

		public T Get<T>(string relativeAddress)
		{
			using(var client = CreateClient())
			{
				var response = client.GetAsync(GetUrl(relativeAddress)).Result;
				if (!response.IsSuccessStatusCode)
					HandleInvalidResponse(response);

				FetchDebugData(response);
				var result = response.Content.ReadAsStringAsync().Result;

				return typeof (T) == typeof (string) 
					? (T) (object) result 
					: JsonConvert.DeserializeObject<T>(result);
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
				var json = JsonConvert.SerializeObject(data);
				var content = new StringContent(json);
				if (!string.IsNullOrEmpty(json))
					content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var response = client.PostAsync(GetUrl(relativeAddress), content).Result;
				if (!response.IsSuccessStatusCode)
					HandleInvalidResponse(response);

				FetchDebugData(response);
				var responseContent = response.Content.ReadAsStringAsync().Result;
				var deserialized = JsonConvert.DeserializeObject<T>(responseContent);
				return deserialized;
			}
		}

		public void Post<T>(string relativeAddress, T data)
		{
			using(var client = CreateClient())
			{
				var json = JsonConvert.SerializeObject(data);
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
				if (string.IsNullOrEmpty(metacoError.metaco_error))
					throw new MetacoClientException(metacoError, ErrorType.UnknownError, content, (int) response.StatusCode, inner);
			}
			catch (Exception e)
			{
				metacoError = new MetacoErrorResult();
				metacoError.metaco_error = "";
				metacoError.status= (int)response.StatusCode;
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
