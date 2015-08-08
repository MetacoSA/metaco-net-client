using System;
using MetacoClient.Contracts;
using NBitcoin;
using Xunit;

namespace MetacoClient.Tests
{
	public class AssetsTest : MetacoClientTestBase
	{
		[Fact]
		public void CanGetAssets()
		{
			var client = GetAnonymousMetacoClient().CreateClient();

			var assets = client.GetAssets();
			Assert.NotNull(assets);
			Assert.True(assets.Length > 0);
		}

		[Fact]
		public void CanGetAsset()
		{
			var client = GetAnonymousMetacoClient().CreateClient();

			var asset = client.GetAsset("MTC:USD");
			Assert.NotNull(asset);
			Assert.Equal(asset.Definition.Ticker, "MTC:USD");
		}

		[Fact]
		public void CannotGetFalseAsset()
		{
			var client = GetAnonymousMetacoClient().CreateClient();
			try
			{
				var asset = client.GetAsset("FAKE:ASSET");
				throw new Exception("A MetacoClientException was expected!");
			}
			catch (MetacoClientException e)
			{
				Assert.Equal(e.ErrorType, ErrorType.InvalidInput);
				Assert.Equal(e.Content, "{\r\n  \"status\": 404,\r\n  \"metaco_error\": \"invalid_input\",\r\n  \"parameter_name\": \"tickerId\",\r\n  \"message\": \"Ticker not found\"\r\n}");
				Assert.Equal(e.MetacoError.message, "Ticker not found");
				Assert.Equal(e.StatusCode, 404);
				Assert.Equal(e.MetacoError.location, null);
				Assert.Equal(e.MetacoError.parameter_name, "tickerId");
			}
		}

		[Fact]
		public void CanGetAssetsHistory()
		{
			var client = GetAnonymousMetacoClient().CreateClient();

			var currentTimestamp = DateTime.UtcNow;
			var timestampThirtyMinutesAgo = currentTimestamp.Subtract(TimeSpan.FromMinutes(30));

			var criteria = new HistoryCriteria(timestampThirtyMinutesAgo.ToUnixTimestamp(), currentTimestamp.ToUnixTimestamp(), "10m", false);

			var historyResult = client.GetAssetsHistory(criteria);
			Assert.NotNull(historyResult);
			Assert.True(historyResult.Assets.Length > 0);
		}

		[Fact]
		public void CanGetSpecificAssetsHistory()
		{
			var client = GetAnonymousMetacoClient().CreateClient();

			var currentTimestamp = DateTime.UtcNow;
			var timestampThirtyMinutesAgo = currentTimestamp.Subtract(TimeSpan.FromMinutes(30));

			var criteria = new HistoryCriteria(timestampThirtyMinutesAgo.ToUnixTimestamp(), currentTimestamp.ToUnixTimestamp(), "10m", false);

			var historyResult = client.GetAssetsHistory(criteria, new[] {"USD"});
			Assert.NotNull(historyResult);
			Assert.True(historyResult.Assets.Length == 1);
		}
	}
}
