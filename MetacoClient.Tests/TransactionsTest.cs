
using System;
using System.Linq;
using Metaco.Client;
using Metaco.Client.Contracts;
using NUnit.Framework;

namespace MetacoClient.Tests
{
	public class TransactionsTest : MetacoClientTestBase
	{
		[Test]
		public void CanProcessAssetTransaction() 
		{
			var addr = GetBitcoinAddress().ToString();
			var newTransaction = new NewTransaction {
				Ticker = "MTC:USD", 
				AmountAsset = 1, 
				From = addr, 
				To = addr, 
				Change = addr, 
				FeePerKB = 12345
			};
			
			var client = CreateAuthenticatedClient();
			var created = client.CreateTransaction(newTransaction);
			Assert.NotNull(created);
			Assert.NotNull(created.Raw);
			Assert.AreEqual(addr, created.InputsToSign.First().SigningAddress);

			var rawTx = new RawTransaction {Raw = GetHexSignedTransaction(created)};
			var result = client.BroadcastTransaction(rawTx);

			Assert.True(result.IsSuccess);
		}

		[Test]
		public void CanProcessBtcTransaction() 
		{
			var addr = GetBitcoinAddress().ToString();
			var newTransaction = new NewTransaction {
				Ticker = "XBT",
				AmountSatoshi = 100000,
				From = addr,
				To = addr,
				Change = addr,
				FeePerKB = 12345,
			};

			var client = CreateAuthenticatedClient();
			var created = client.CreateTransaction(newTransaction);
			Assert.NotNull(created);
			Assert.NotNull(created.Raw);
			Assert.AreEqual(addr, created.InputsToSign.First().SigningAddress);

			var rawTx = new RawTransaction{ Raw = GetHexSignedTransaction(created)};
			var result = client.BroadcastTransaction(rawTx);

			Assert.True(result.IsSuccess);
		}


		[Test]
		public void CantBroadcastTransaction() 
		{
			try {
				var client = CreateAuthenticatedClient();

				var raw = new RawTransaction {Raw = "fakerawtx"};
				client.BroadcastTransaction(raw);
				throw new Exception("An MetacoException was expected!");
			} catch (MetacoClientException e) {
				Assert.AreEqual(ErrorType.InvalidInput, e.ErrorType);
			}
		}

		[Test]
		public void CanGetWalletDetails() 
		{
			var addr = GetBitcoinAddress().ToString();
			var client = CreateAuthenticatedClient();
			var walletDetails = client.GetWalletDetails(addr);
			Assert.NotNull(walletDetails);
			Assert.AreEqual(addr, walletDetails.Addresses.First());
		}

		[Test]
		public void CanGetPaginatedWalletDetails()
		{
			var addr = GetBitcoinAddress().ToString();
			var client = CreateAuthenticatedClient();

			var walletDetails = client.GetWalletDetails(addr, Page.Create(0,1));
			Assert.NotNull(walletDetails);
			Assert.AreEqual(1, walletDetails.Transactions.Count());
			Assert.AreEqual(addr, walletDetails.Addresses.First());
		}
	}
}
