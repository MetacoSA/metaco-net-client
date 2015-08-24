using System;
using Newtonsoft.Json;

namespace Metaco.Client.Contracts
{
	public class PageDetails
	{
		[JsonProperty("number")]
		public int Number
		{
			get; 
			set;
		}
		[JsonProperty("size")]
		public int Size
		{
			get; 
			set;
		}
	}

	public class Page
	{
		public int Number
		{
			get;
			private set;
		}
		public int Size
		{
			get;
			private set;
		}

		public static Page NoPagination = new Page(0, 0);

		private Page(int page, int size)
		{
			Number = page;
			Size = size;
		}

		public static Page Create(int page, int size)
		{
			if (page < 0)
				throw new ArgumentOutOfRangeException("page", "page number must be a possitive value");
			if (size < 1)
				throw new ArgumentOutOfRangeException("size", "page size must be greater than 1");
			return new Page(page, size);
		}

		public string ToQueryString()
		{
			return (Number == 0 && Size == 0)  
				? string.Empty
				:string.Format("?pageNumber={0}&pageSize={1}", Number, Size);
		}
	}
}
