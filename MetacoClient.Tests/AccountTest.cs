using System;
using Metaco.Client;
using Metaco.Client.Contracts;
using NUnit.Framework;

namespace MetacoClient.Tests
{
	[TestFixture]
	public class AccountTest : MetacoClientTestBase 
	{
		[Test]
		public void ClientCanRegisterAndValidateAccount()
		{
			var client = CreateClient();

			/** Account registration **/
			var result = client.RegisterAccount(new RegisterAccountRequest {Phone = "+15005550006"});
			Assert.NotNull(result.ApiId);

			var validationCode = client.LatestDebugData;

			client = CreateClient(result.ApiId,result.ApiKey);

			/** Account Validation **/
			client.ConfirmPhoneNumber(new ValidateAccountRequest {Code = validationCode});

			/** Account Check **/
			var status = client.GetAccountStatus();
			Assert.True(status.Kyc1);

			/** Can't double validate account **/
			try 
			{
				client.ConfirmPhoneNumber(new ValidateAccountRequest{Code = validationCode});
				throw new Exception("Cannot double validate account!");
			} 
			catch (MetacoClientException e) 
			{
				Assert.AreEqual(e.ErrorType, ErrorType.PhoneConfirmationNotFound);
			}
		}


		[Test]
		public void ClientCantRegisterAccount() 
		{
			try 
			{
				var client = CreateClient();
				var result = client.RegisterAccount(new RegisterAccountRequest{ Phone = ""});
				throw new Exception("Cannot double validate account!");
			} 
			catch (MetacoClientException e) 
			{
				Assert.AreEqual(ErrorType.SmsSendingFailed, e.ErrorType);
			}
		}

		[Test]
		public void ClientCantGetAccountStatus() {
			try 
			{
				var client = CreateClient();

				var status = client.GetAccountStatus();
				throw new Exception("An MetacoClientException was expected");
			}
			catch (MetacoClientException e)
			{
				Assert.AreEqual(ErrorType.Unauthorized, e.ErrorType);
			}
		}
	}
}