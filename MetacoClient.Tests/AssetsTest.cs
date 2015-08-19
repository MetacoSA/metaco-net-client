using System;
using System.Linq;
using MetacoClient.Contracts;
using NUnit.Framework;

namespace MetacoClient.Tests
{
	[TestFixture]
	public class AssetsTest : MetacoClientTestBase
	{
		[Test]
		public void CanGetAssets()
		{
			var client = CreateClient();

			var assets = client.GetAssets();
			Assert.NotNull(assets);
			Assert.True(assets.Length > 0);
		}

		[Test]
		public void CanGetAsset()
		{
			var client = CreateClient();

			var asset = client.GetAsset("MTC:USD");
			Assert.NotNull(asset);
			Assert.AreEqual("MTC:USD", asset.Definition.Ticker);
		}

		[Test]
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
				Assert.AreEqual(e.ErrorType, ErrorType.InvalidInput);
				Assert.AreEqual(e.Content, "{\r\n  \"status\": 404,\r\n  \"metaco_error\": \"invalid_input\",\r\n  \"parameter_name\": \"tickerId\",\r\n  \"message\": \"Ticker not found\"\r\n}");
				Assert.AreEqual(e.MetacoError.Message, "Ticker not found");
				Assert.AreEqual(e.StatusCode, 404);
				Assert.AreEqual(e.MetacoError.Location, null);
				Assert.AreEqual(e.MetacoError.ParameterName, "tickerId");
			}
		}
	}
}
