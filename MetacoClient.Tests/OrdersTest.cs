using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MetacoClient.Contracts;
using Xunit;

namespace MetacoClient.Tests
{
	public class OrdersTest : MetacoClientTestBase
	{

		[Fact]
		public void CanProcessOrder()
		{
			var client = CreateAuthenticatedClient();

			var newOrder = new NewOrder {
				AmountAsset = 100L, 
				Funding = new List<string>(new []{ GetBitcoinAddress().ToString()}),
				Recipient = GetBitcoinAddress().ToString(),
				Ticker = "MTC:USD",
				Type = OrderType.Buy
			};

			var created = client.CreateOrder(newOrder);
			Assert.NotNull(created);
			Assert.Equal(100L, created.AmountAsset);

			var orderToSign = WaitForOrderState(client, created.Id, "Signing");
			if (orderToSign == null) {
				throw new Exception("Order " + created.Id + " took to long to go to Signing state");
			}

			/** Signing and submit **/
			var rawTx = new RawTransaction {
				Raw = GetHexSignedTransaction(orderToSign.Transaction)
			};

			client.SubmitSignedOrder(orderToSign.Id, rawTx);

			/** Wait for broadcasting **/
			var unconfirmed = WaitForOrderState(client, created.Id, "Unconfirmed");
			if (unconfirmed == null) {
				throw new Exception("Order " + created.Id + " took to long to go to Unconfirmed state");
			}

			Assert.Equal(1, unconfirmed.AmountAsset);

			/** Try to delete broadcasting order **/
			try {
				client.CancelOrder(unconfirmed.Id);
			} catch (MetacoClientException e) {
				Assert.Equal(ErrorType.OrderNotCancellable, e.ErrorType);
			}


			/** Fetch all the orders **/
			var orders = client.GetOrders();

			if (orders.Orders.All(x => x.Id != created.Id)) 
			{
				throw new Exception("Order " + created.Id + " is not present in orders list");
			}
		}

		[Fact]
		public void CanCancelOrder() 
		{
			var client = CreateAuthenticatedClient();
			var funding = GetBitcoinAddress().ToString();

			var newOrder = new NewOrder {
				AmountAsset = 100L, 
				Funding = new[] {funding}, 
				Recipient = GetBitcoinAddress().ToString(), 
				Ticker = "MTC:USD", 
				Type = OrderType.Buy
			};

			var created = client.CreateOrder(newOrder);
			Assert.NotNull(created);
			Assert.Equal(100, created.AmountAsset);

			client.CancelOrder(created.Id);

			/** Wait for cancel **/
			var canceled = WaitForOrderState(client, created.Id, "Canceled");
			if (canceled == null) {
				throw new Exception("Order " + created.Id + " took to long to go to Canceled state");
			}
			Assert.Equal("explicit_cancel", canceled.CancelReason);
			Assert.Equal(canceled.Status, "Canceled");
		}

		private Order WaitForOrderState(MetacoClient client, string orderId, string status)
		{
			var remainingTries = 15;
			var orderReady = false;
			Order order;
			do {
				Thread.Sleep(2000);
				order = client.GetOrder(orderId);

				if (order.Status == status) {
					orderReady = true;
				}

				remainingTries--;

				if (remainingTries == 0) {
					return null;
				}
			} while (!orderReady);
			return order;
		}
	}
}
