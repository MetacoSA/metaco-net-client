using System;

namespace MetacoClient
{
	public class MetacoErrorResult
	{
		public int status { get; set; }
		public String metaco_error { get; set; }
		public String location { get; set; }
		public String parameter_name { get; set; }
		public String message { get; set; }
	}
}

