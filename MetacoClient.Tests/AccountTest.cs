using System;
using MetacoClient.Contracts;
using Xunit;

namespace MetacoClient.Tests
{
	public class AccountTest : MetacoClientTestBase 
	{
		[Fact]
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
				Assert.Equal(e.ErrorType, ErrorType.PhoneConfirmationNotFound);
			}
		}


		[Fact]
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
				Assert.Equal(ErrorType.SmsSendingFailed, e.ErrorType);
			}
		}

		[Fact]
		public void ClientCantGetAccountStatus() {
			try 
			{
				var client = CreateClient();

				var status = client.GetAccountStatus();
				throw new Exception("An MetacoClientException was expected");
			}
			catch (MetacoClientException e)
			{
				Assert.Equal(ErrorType.Unauthorized, e.ErrorType);
			}
		}
	}
}