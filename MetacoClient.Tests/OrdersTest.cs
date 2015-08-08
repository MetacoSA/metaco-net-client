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
			var client = GetAuthenticatedMetacoClient().CreateClient();

			var newOrder = new NewOrder {
				AmountAsset = 1, 
				Change = "", 
				Funding = new List<string>(new []{ GetBitcoinAddress().ToString()}),
				Recipient = GetBitcoinAddress().ToString(),
				Ticker = "MTC:USD",
				Type = "bid"
			};

			var created = client.CreateOrder(newOrder);
			Assert.NotNull(created);
			Assert.Equal(created.AmountAsset, 1L);

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

			Assert.Equal(1, (long)unconfirmed.AmountAsset);

			/** Try to delete broadcasting order **/
			try {
				client.CancelOrder(unconfirmed.Id);
			} catch (MetacoClientException e) {
				Assert.Equal(e.ErrorType, ErrorType.OrderNotCancellable);
			}


			/** Fetch all the orders **/

			var orders = client.GetOrders();

			if (orders.Orders.All(x => x.Id != created.Id)) 
			{
				throw new Exception("Order " + created.Id + " is not present in orders list");
			}
		}
#if false
		[Fact]
		public void clientCanCancelOrder() throws MetacoClientException, InterruptedException {
			global::MetacoClient client = TestUtils.GetMetacoAuthenticatedClientTestBuilder()
				.makeClient();

			NewOrder newOrder = new NewOrder();
			newOrder.setAmountAsset(1L);
			newOrder.setChange("");
			List<String> funding = new ArrayList<String>();
			funding.add(TestUtils.GetBitcoinAddress());
			newOrder.setFunding(funding);
			newOrder.setRecipient(TestUtils.GetBitcoinAddress());
			newOrder.setTicker("MTC:USD");
			newOrder.setType("bid");

			Order created = client.createOrder(newOrder);
			Assert.assertNotNull(created);
			Assert.assertNotNull(created.getAmountAsset());
			Assert.assertEquals((long)created.getAmountAsset(), 1);

			client.cancelOrder(created.getId());

			/** Wait for cancel **/
			Order canceled = WaitForOrderState(client, created.getId(), "Canceled");
			if (canceled == null) {
				Assert.fail("Order " + created.getId() + " took to long to go to Canceled state");
			}
			Assert.assertEquals(canceled.getCancelReason(), "explicit_cancel");
			Assert.assertEquals("Canceled", canceled.getStatus());
		}
#endif

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
