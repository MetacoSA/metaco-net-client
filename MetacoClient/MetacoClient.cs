using System;
using System.Collections.Generic;
using MetacoClient.Contracts;
using MetacoClient.Http;

namespace MetacoClient
{
	public class RestClient
	{
		private readonly MetacoHttpClient _httpClient;

		public string LatestDebugData
		{
			get { return _httpClient.DebugInfo; }
		}

		public RestClient(string apiUrl)
			: this(apiUrl, false)
		{
		}

		public RestClient(string apiUrl, bool testingMode)
			: this(apiUrl, null, null, testingMode)
		{
		}

		public RestClient(string apiUrl, string apiId, string apiKey)
			: this(apiUrl, apiId, apiKey, false)
		{
		}

		public RestClient(string apiUrl, string apiId, string apiKey, bool testingMode)
			: this(new Uri(apiUrl), apiId, apiKey, testingMode)
		{
		}

		public RestClient(Uri apiUrl, string apiId, string apiKey, bool testingMode)
		{
			if(apiUrl == null)
				throw new ArgumentNullException("apiUrl");
			_httpClient = new MetacoHttpClient(apiId, apiKey, apiUrl, testingMode);
		}


		/// <summary>
		/// Register an account on Metaco
		/// Sends an SMS to the provided phone number
		/// If you are in debug mode, this request will return a HTTP header X-Metaco-DebugData with the validation code, it won't be send by SMS
		/// </summary>
		/// <param name="request">The request containing the phone number.</param>
		/// <returns>The initial account settings</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <see cref="http://docs.metaco.apiary.io/#reference/account/account-management/register-an-account">Online Documentation</see>
		public AccountRegistrationResult RegisterAccount(RegisterAccountRequest request)
		{
			return _httpClient.Post<AccountRegistrationResult, RegisterAccountRequest>("account", request);
		}

		/// <summary>
		/// Return the details of an account (API Id, KYC Status and remaining trading amount)
		/// </summary>
		/// <returns>The account details</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <see cref="http://docs.metaco.apiary.io/#reference/account/account-management/get-account-status">Online Documentation</see>
		/// <remarks>Requires Authentication</remarks>
		public AccountStatus GetAccountStatus()
		{
			return _httpClient.Get<AccountStatus>("account");
		}

		/// <summary>
		/// Validate the authenticated account, throws an exception if there is an error
		/// </summary>
		/// <exception cref="MetacoClientException"></exception>
		/// <see cref="http://docs.metaco.apiary.io/#reference/account/confirm-a-registration/confirm-a-phone-number">Online Documentation</see>
		/// <remarks>Requires Authentication</remarks>
		public void ConfirmPhoneNumber(ValidateAccountRequest request)
		{
			if (request == null) 
				throw new ArgumentNullException("request");

			_httpClient.Post("account/confirmation", request);
		}

		/// <summary>
		/// Returns all the available Assets and their details
		/// </summary>
		/// <returns>The assets array</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <see cref="http://docs.metaco.apiary.io/#reference/assets/assets-list/list-all-assets">Online Documentation</see>
		public Asset[] GetAssets()
		{
			return _httpClient.Get<Asset[]>("assets");
		}

		/// <summary>
		/// Returns the selected Asset if it exists and its details
		/// </summary>
		/// <param name="ticker">The asset ticker</param>
		/// <returns>The asset object</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <see cref="http://docs.metaco.apiary.io/#reference/assets/asset-information/retrieve-an-asset">Online Documentation</see>
		/// <remarks>Requires Authentication</remarks>
		public Asset GetAsset(string ticker)
		{
			if (string.IsNullOrEmpty(ticker)) 
				throw new ArgumentNullException("ticker");

			return _httpClient.Get<Asset>(string.Format("assets/{0}", ticker));
		}

		/// <summary>
		/// Returns the history for all the available assets according to the given criteria
		/// </summary>
		/// <param name="criteria">The search criteria</param>
		/// <returns>The history object</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <see cref="http://docs.metaco.apiary.io/#reference/assets/asset-information/retrieve-an-asset">Online Documentation</see>
		/// <remarks>Requires Authentication</remarks>
		public AssetsHistoryResult GetAssetsHistory(HistoryCriteria criteria)
		{
			if (criteria == null) 
				throw new ArgumentNullException("criteria");

			return GetAssetsHistory(criteria, new string[0]);
		}

		/// <summary>
		/// Returns the history for the provided assets according to the given criteria
		/// </summary>
		/// <param name="criteria">The search criteria</param>
		///<param name="tickers">The assets ticker</param>
		///<returns>The history object</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <see cref="http://docs.metaco.apiary.io/#reference/assets/assets-history/retrieve-history-of-all-assets">Online Documentation</see>
		/// <remarks>Assets must be given using this format : USD,XAU,etc..</remarks>
		public AssetsHistoryResult GetAssetsHistory(HistoryCriteria criteria, IEnumerable<string> tickers)
		{
			if (criteria == null) 
				throw new ArgumentNullException("criteria");
			if (tickers == null) 
				throw new ArgumentNullException("tickers");

			var tickersStr = string.Join(",", tickers);
			tickersStr = !string.IsNullOrEmpty(tickersStr) ? tickersStr : "all";
			const string urlTemplate = "assets/history?underlyings={0}&from={1}&to={2}&freq={3}&orderAsc={4}";

			return _httpClient.Get<AssetsHistoryResult>(
				string.Format(urlTemplate, 
					tickersStr, 
					criteria.From.ToEpoch(), 
					criteria.To.ToEpoch(), 
					criteria.Freq, 
					criteria.OrderAsc));

		}

		/// <summary>
		/// Create an order using the provided parameters
		/// This order will be processed in our system
		/// It will require your signature later when the trade state will be Signing
		/// </summary>
		///<param name="createOrder">The order to be created</param>
		///<returns>The history object</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <see cref="http://docs.metaco.apiary.io/#reference/orders/orders-management/request-an-order">Online Documentation</see>
		/// <remarks>Requires Authentication</remarks>
		public Order CreateOrder(NewOrder createOrder)
		{
			if (createOrder == null) 
				throw new ArgumentNullException("createOrder");

			return _httpClient.Post<Order, NewOrder>("orders", createOrder);
		}

		/// <summary>
		/// Returns the user's orders
		/// </summary>
		/// <returns>The orders array</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <see cref="http://docs.metaco.apiary.io/#reference/orders/orders-management/request-an-order">Online Documentation</see>
		/// <remarks>Requires Authentication</remarks>
		public OrderResultPage GetOrders()
		{
			return _httpClient.Get<OrderResultPage>("orders");
		}

		/// <summary>
		/// Returns the specified user's order
		/// </summary>
		/// <param name="id">The order identifier.</param>
		/// <returns>The order object </returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <remarks>Requires Authentication </remarks>
		/// <see cref="http://docs.metaco.apiary.io/#reference/orders/order-information/retreive-an-order">Online Documentation</see>
		public Order GetOrder(string id)
		{
			if (string.IsNullOrEmpty(id)) 
				throw new ArgumentNullException("id");

			return _httpClient.Get<Order>(string.Format("orders/{0}", id));
		}

		/// <summary>
		/// Submit a signed order
		/// </summary>
		/// <param name="id">The order identifier.</param>
		/// <param name="rawTransaction">The raw-signed transaction</param>
		/// <returns>The order object </returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <remarks>
		/// Requires Authentication
		/// You have to sign each of your inputs of the selected order (you will get those details by fetching the orders)
		/// Then encode the transaction in hexadecimal and send it here
		/// </remarks>
		/// <see cref="http://docs.metaco.apiary.io/#reference/orders/order-information/submit-a-signed-order">Online Documentation</see>
		public Order SubmitSignedOrder(string id, RawTransaction rawTransaction)
		{
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");
			if (rawTransaction == null)
				throw new ArgumentNullException("rawTransaction");

			return _httpClient.Post<Order, RawTransaction>(string.Format("orders/{0}", id), rawTransaction);
		}

		/// <summary>
		/// Cancel the specified order
		/// </summary>
		/// <param name="id">The order identifier.</param>
		/// <exception cref="MetacoClientException"></exception>
		/// <remarks>
		/// Requires Authentication
		/// </remarks>
		/// <see cref="http://docs.metaco.apiary.io/#reference/orders/order-information/cancel-an-order">Online Documentation</see>
		public void CancelOrder(string id)
		{
			if (string.IsNullOrEmpty(id))
				throw new ArgumentNullException("id");

			_httpClient.Delete("orders/" + id);
		}

		/// <summary>
		/// Create a Transaction using the provided parameters
		/// </summary>
		/// <param name="tx">The raw-signed transaction</param>
		/// <returns>The transaction to sign</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <remarks>
		/// Requires Authentication
		/// </remarks>
		/// <see cref="http://docs.metaco.apiary.io/#reference/transactions/raw-transaction/get-a-raw-transaction">Online Documentation</see>
		public TransactionToSign CreateTransaction(NewTransaction tx)
		{
			if (tx == null) 
				throw new ArgumentNullException("tx");

			var parms = new List<string>();
			
			if (tx.AmountAsset  > 0)
			{
				parms.Add("amount_asset=" + tx.AmountAsset );
			}
			if (tx.AmountSatoshi > 0)
			{
				parms.Add("amount_satoshi=" + tx.AmountSatoshi);
			}
			if (!string.IsNullOrEmpty(tx.Change))
			{
				parms.Add("change=" + tx.Change);
			}
			if (!string.IsNullOrEmpty(tx.From))
			{
				parms.Add("from=" + tx.From);
			}
			if (!string.IsNullOrEmpty(tx.To))
			{
				parms.Add("to=" +  tx.To);
			}
			if (!string.IsNullOrEmpty(tx.Ticker))
			{
				parms.Add("ticker=" + tx.Ticker);
			}
			if (tx.FeePerKB > 0)
			{
				parms.Add("feePerKB=" + tx.FeePerKB);
			}

			return _httpClient.Get<TransactionToSign>("transactions/raw?" + string.Join("&", parms));
		}

		/// <summary>
		/// Submit a signed transaction
		/// </summary>
		/// <param name="rawTransaction">The raw-signed transaction to be broadcasted</param>
		/// <returns>The broadcasting operation result</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <remarks>
		/// Requires Authentication
		/// You have to sign each of your inputs of the selected transaction (you will get those details when creating the transaction through Metaco)
		/// Then encode the transaction in hexadecimal and send it here
		/// </remarks>
		/// <see cref="http://docs.metaco.apiary.io/#reference/transactions/transaction-broadcast/broadcast-a-transaction">Online Documentation</see>
		public TransactionBroadcastResult BroadcastTransaction(RawTransaction rawTransaction)
		{
			if (rawTransaction == null) 
				throw new ArgumentNullException("rawTransaction");

			return _httpClient.Post<TransactionBroadcastResult, RawTransaction>("transactions", rawTransaction);
		}

		/// <summary>
		/// Returns the current wallet state
		/// Contains the current balances, the values and the transaction history
		/// </summary>
		/// <param name="address"></param>
		/// <returns>The wallet details</returns>
		/// <exception cref="MetacoClientException"></exception>
		/// <remarks>
		/// Requires Authentication
		/// </remarks>
		/// <see cref="http://docs.metaco.apiary.io/#reference/transactions/transaction-broadcast/fetch-wallet-information">Online Documentation</see>
		public WalletDetails GetWalletDetails(string address)
		{
			if (string.IsNullOrEmpty(address)) 
				throw new ArgumentNullException("address");

			return _httpClient.Get<WalletDetails>("transactions/" + address);
		}
	}
}
