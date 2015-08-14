using System;
using System.Linq;
using System.Threading;
using MetacoClient.Contracts;
using NUnit.Framework;

namespace MetacoClient.Tests
{
	[TestFixture]
	public class OrdersTest : MetacoClientTestBase
	{
		[Test]
		public void CanProcessOrder()
		{
			var addr = GetBitcoinAddress().ToString();
			var newOrder = new NewOrder {
				AmountAsset = 100L, 
				Funding = new []{ addr },
				Recipient = addr,
				Ticker = "MTC:USD",
				Type = OrderType.Buy
			};

			var client = CreateAuthenticatedClient();

			var created = client.CreateOrder(newOrder);
			Assert.NotNull(created);
			Assert.AreEqual(100L, created.AmountAsset);

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

			Assert.AreEqual(100, unconfirmed.AmountAsset);

			/** Try to delete broadcasting order **/
			try {
				client.CancelOrder(unconfirmed.Id);
				throw new Exception("A MetacoClientException was expected!");
			} catch (MetacoClientException e) {
				Assert.AreEqual(ErrorType.OrderNotCancellable, e.ErrorType);
			}


			/** Fetch all the orders **/
			var orders = client.GetOrders();

			if (orders.Orders.All(x => x.Id != created.Id)) 
			{
				throw new Exception("Order " + created.Id + " is not present in orders list");
			}
		}

		[Test]
		public void CanCancelOrder() 
		{
			var funding = GetBitcoinAddress().ToString();

			var newOrder = new NewOrder {
				AmountAsset = 100L, 
				Funding = new[] {funding}, 
				Recipient = GetBitcoinAddress().ToString(), 
				Ticker = "MTC:USD", 
				Type = OrderType.Buy
			};

			var client = CreateAuthenticatedClient();
			var created = client.CreateOrder(newOrder);
			Assert.NotNull(created);
			Assert.AreEqual(100, created.AmountAsset);

			client.CancelOrder(created.Id);

			/** Wait for cancel **/
			var canceled = WaitForOrderState(client, created.Id, "Canceled");
			if (canceled == null) {
				throw new Exception("Order " + created.Id + " took to long to go to Canceled state");
			}
			Assert.AreEqual("explicit_cancel", canceled.CancelReason);
			Assert.AreEqual("Canceled", canceled.Status);
		}

		[Test]
		public void CanGetPaginatedOrders()
		{
			var client = CreateAuthenticatedClient();

			var orders = client.GetOrders(Page.Create(0, 1));
			Assert.NotNull(orders);
			Assert.AreEqual(1, orders.Orders.Count());
		}

		private Order WaitForOrderState(RestClient client, string orderId, string status)
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
