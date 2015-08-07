using System.Configuration;
using MetacoClient.Contracts;
using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;
using Transaction = NBitcoin.Transaction;

namespace MetacoClient.Tests
{
	public class MetacoClientTestBase
	{
		private const string METACO_ENV_API_URL_NAME = "METACO_ENV_API_URL";
		private const string METACO_ENV_API_ID_NAME = "METACO_ENV_API_ID";
		private const string METACO_ENV_API_KEY_NAME = "METACO_ENV_API_KEY";
		private const string METACO_ENV_WALLET_PRIVATE_KEY_HEX_NAME = "METACO_ENV_WALLET_PRIVATE_KEY_HEX";

	    private const string DEFAULT_API_URL = "http://api.testnet.metaco.com/v1/";


		public static MetacoClientBuilder GetAnonymousMetacoClient()
		{
			var apiUrl = ConfigurationManager.AppSettings[METACO_ENV_API_URL_NAME] ?? DEFAULT_API_URL;
			return new MetacoClientBuilder()
					.WithApiUrl(apiUrl)
					.WithTestingMode(true);
		}

		public static MetacoClientBuilder GetAuthenticatedMetacoClient()
		{
			var apiUrl = ConfigurationManager.AppSettings[METACO_ENV_API_URL_NAME] ?? DEFAULT_API_URL;
			var apiId = ConfigurationManager.AppSettings[METACO_ENV_API_ID_NAME];
			var apiKey = ConfigurationManager.AppSettings[METACO_ENV_API_KEY_NAME];

			return new MetacoClientBuilder()
					.WithApiUrl(apiUrl)
					.WithApiId(apiId)
					.WithApiKey(apiKey)
					.WithTestingMode(true);
	    }

		public static BitcoinAddress GetBitcoinAddress() 
		{
			var walletPrivateKey = ConfigurationManager.AppSettings[METACO_ENV_WALLET_PRIVATE_KEY_HEX_NAME];

			var key = new ECKey(Encoders.Hex.DecodeData(walletPrivateKey), true);
			return key.GetPubKey(false).GetAddress(Network.TestNet);
		}

		public static string GetHexSignedTransaction(TransactionToSign txToSign) {
			var walletPrivateKey = ConfigurationManager.AppSettings[METACO_ENV_WALLET_PRIVATE_KEY_HEX_NAME];

			var key = new Key(Encoders.Hex.DecodeData(walletPrivateKey));
			var addr = key.PubKey.GetAddress(Network.TestNet);
			//var scriptPubKey = Script.CreateFromDestinationAddress(addr);

			var tx = new Transaction(Encoders.Hex.DecodeData(txToSign.Raw));
			var tb = new TransactionBuilder();
			var signedTx = tb
				.ContinueToBuild(tx)
				.AddKeys(key)
				.SignTransaction(tx);

			
			tb.Verify(signedTx);

			return signedTx.ToHex();
		}
	}
}