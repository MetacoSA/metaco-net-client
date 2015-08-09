using System.Configuration;
using MetacoClient.Contracts;
using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;
using Xunit;
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


		public static MetacoClient CreateClient()
		{
			var apiUrl = ConfigurationManager.AppSettings[METACO_ENV_API_URL_NAME] ?? DEFAULT_API_URL;
			return new MetacoClient(apiUrl, true);
		}

		public static MetacoClient CreateClient(string apiId, string apiKey)
		{
			var apiUrl = ConfigurationManager.AppSettings[METACO_ENV_API_URL_NAME] ?? DEFAULT_API_URL;
			return new MetacoClient(apiUrl, apiId, apiKey, true);
	    }

		public static MetacoClient CreateAuthenticatedClient()
		{
			var apiUrl = ConfigurationManager.AppSettings[METACO_ENV_API_URL_NAME] ?? DEFAULT_API_URL;
			var apiId = ConfigurationManager.AppSettings[METACO_ENV_API_ID_NAME] ?? DEFAULT_API_URL;
			var apiKey = ConfigurationManager.AppSettings[METACO_ENV_API_KEY_NAME] ?? DEFAULT_API_URL;
			return new MetacoClient(apiUrl, apiId, apiKey, true);
	    }

		public static BitcoinAddress GetBitcoinAddress() 
		{
			var walletPrivateKey = ConfigurationManager.AppSettings[METACO_ENV_WALLET_PRIVATE_KEY_HEX_NAME];

			var key = new ECKey(Encoders.Hex.DecodeData(walletPrivateKey), true);
			var k = new Key(Encoders.Hex.DecodeData(walletPrivateKey));
			return k.PubKey.GetAddress(Network.TestNet);
		}

		public static string GetHexSignedTransaction(TransactionToSign txToSign) 
		{
			var walletPrivateKey = ConfigurationManager.AppSettings[METACO_ENV_WALLET_PRIVATE_KEY_HEX_NAME];

			var key = new Key(Encoders.Hex.DecodeData(walletPrivateKey));

			var scriptPubKey = PayToPubkeyHashTemplate.Instance.GenerateScriptPubKey(key.PubKey);
			var tx = new Transaction(Encoders.Hex.DecodeData(txToSign.Raw));

			foreach(var inputsToSign in txToSign.InputsToSign)
			{
				var sigHash = tx.GetSignatureHash(scriptPubKey, inputsToSign.Index);
				var sig = key.Sign(sigHash);

				var txSign = new TransactionSignature(sig, SigHash.All);
				var inputScript = PayToPubkeyHashTemplate.Instance.GenerateScriptSig(txSign, key.PubKey);

				tx.Inputs[inputsToSign.Index].ScriptSig = inputScript;
				Assert.True(Script.VerifyScript(scriptPubKey, tx, inputsToSign.Index));
			}

			return tx.ToHex();
		}
	}
}