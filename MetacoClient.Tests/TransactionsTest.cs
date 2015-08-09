
using System;
using System.Linq;
using MetacoClient.Contracts;
using Xunit;

namespace MetacoClient.Tests
{
	public class TransactionsTest : MetacoClientTestBase
	{
		[Fact]
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
			Assert.Equal(addr, created.InputsToSign.First().SigningAddress);

			var rawTx = new RawTransaction {Raw = GetHexSignedTransaction(created)};
			var result = client.BroadcastTransaction(rawTx);

			Assert.True(result.IsSuccess);
		}

		[Fact]
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
			Assert.Equal(addr, created.InputsToSign.First().SigningAddress);

			var rawTx = new RawTransaction{ Raw = GetHexSignedTransaction(created)};
			var result = client.BroadcastTransaction(rawTx);

			Assert.True(result.IsSuccess);
		}


		[Fact]
		public void CantBroadcastTransaction() 
		{
			try {
				var client = CreateAuthenticatedClient();

				var raw = new RawTransaction {Raw = "fakerawtx"};
				client.BroadcastTransaction(raw);
				throw new Exception("An MetacoException was expected!");
			} catch (MetacoClientException e) {
				Assert.Equal(ErrorType.InvalidInput, e.ErrorType);
			}
		}

		[Fact]
		public void CanGetWalletDetails() 
		{
			var addr = GetBitcoinAddress().ToString();
			var client = CreateAuthenticatedClient();
			var walletDetails = client.GetWalletDetails(addr);
			Assert.NotNull(walletDetails);
			Assert.Equal(addr, walletDetails.Addresses.First());
		}
	}
}
