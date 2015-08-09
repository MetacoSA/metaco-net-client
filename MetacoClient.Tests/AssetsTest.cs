using System;
using System.Linq;
using MetacoClient.Contracts;
using Xunit;

namespace MetacoClient.Tests
{
	public class AssetsTest : MetacoClientTestBase
	{
		[Fact]
		public void CanGetAssets()
		{
			var client = CreateClient();

			var assets = client.GetAssets();
			Assert.NotNull(assets);
			Assert.True(assets.Length > 0);
		}

		[Fact]
		public void CanGetAsset()
		{
			var client = CreateClient();

			var asset = client.GetAsset("MTC:USD");
			Assert.NotNull(asset);
			Assert.Equal("MTC:USD", asset.Definition.Ticker);
		}

		[Fact]
		public void CannotGetFalseAsset()
		{
			var client = CreateClient();
			try
			{
				var asset = client.GetAsset("FAKE:ASSET");
				throw new Exception("A MetacoClientException was expected!");
			}
			catch (MetacoClientException e)
			{
				Assert.Equal(e.ErrorType, ErrorType.InvalidInput);
				Assert.Equal(e.Content, "{\r\n  \"status\": 404,\r\n  \"metaco_error\": \"invalid_input\",\r\n  \"parameter_name\": \"tickerId\",\r\n  \"message\": \"Ticker not found\"\r\n}");
				Assert.Equal(e.MetacoError.Message, "Ticker not found");
				Assert.Equal(e.StatusCode, 404);
				Assert.Equal(e.MetacoError.Location, null);
				Assert.Equal(e.MetacoError.ParameterName, "tickerId");
			}
		}

		[Fact]
		public void CanGetAssetsHistory()
		{
			var client = CreateClient();

			var criteria = new HistoryCriteria(DateTimeOffset.UtcNow.AddSeconds(-30 * 60), DateTimeOffset.UtcNow, "10m", false);

			var historyResult = client.GetAssetsHistory(criteria);
			Assert.NotNull(historyResult);
			Assert.True(historyResult.Assets.Any());
		}

		[Fact]
		public void CanGetSpecificAssetsHistory()
		{
			var client = CreateClient();

			var criteria = new HistoryCriteria(DateTimeOffset.UtcNow.AddSeconds(-30 * 60), DateTimeOffset.UtcNow, "10m", false);

			var historyResult = client.GetAssetsHistory(criteria, new[] {"USD"});
			Assert.NotNull(historyResult);
			Assert.Equal(1, historyResult.Assets.Count());
		}
	}
}
