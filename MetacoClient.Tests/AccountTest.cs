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
			var client = GetAnonymousMetacoClient().CreateClient();

			/** Account registration **/
			var result = client.RegisterAccount(new RegisterAccountRequest {Phone = "+15005550006"});
			Assert.NotNull(result.ApiId);

			var validationCode = client.LatestDebugData;

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
				var client = GetAnonymousMetacoClient().CreateClient();
				var result = client.RegisterAccount(new RegisterAccountRequest());
				throw new Exception("Cannot double validate account!");
			} 
			catch (MetacoClientException e) 
			{
				Assert.Equal(e.ErrorType, ErrorType.SmsSendingFailed);
			}
		}

		[Fact]
		public void ClientCantGetAccountStatus() {
			try 
			{
				var client = GetAnonymousMetacoClient().CreateClient();

				var status = client.GetAccountStatus();
			} catch (MetacoClientException e) {
				Assert.Equal(e.ErrorType, ErrorType.Unauthorized);
			}
		}
	}
}