using System;

namespace MetacoClient
{
	[Serializable]
	public class MetacoClientException : Exception 
	{
		public string Content { get; private set; }
		public int StatusCode { get; private set; }
		public MetacoErrorResult MetacoError { get; private set; }
		public ErrorType ErrorType { get; private set; }

		public MetacoClientException(MetacoErrorResult errorResult, ErrorType errorType, string content, int statusCode) 
			: this(errorResult, errorType, content, statusCode, null)
		{
		}

		public MetacoClientException(MetacoErrorResult errorResult, ErrorType errorType, string content, int statusCode, Exception inner)
			: base(string.Format("Metaco API failed with \"{0}\". {1}", errorResult.Message, (inner!=null ? "See inner exception": "")), inner)
		{
			this.MetacoError = errorResult;
			this.ErrorType = errorType;
			this.Content = content;
			this.StatusCode = statusCode;
		}

	}
}
