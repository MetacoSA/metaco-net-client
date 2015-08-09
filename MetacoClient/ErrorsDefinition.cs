using System;
using System.Collections.Generic;
using System.Linq;

namespace MetacoClient
{
	public enum ErrorType
	{
		InvalidInput,
		ApiCallsQuotaExceeded,
		SmsSendingFailed,
		PhoneConfirmationNotFound,
		InvalidConfirmationCode,
		OrderNotFound,
		NotEnoughFunds,
		OrderTooSmall,
		OrderCountLimitExceeded,
		YearlyTransactionQuotaExceeded,
		MaximumTransactionAmountExceeded,
		OrderNotCancellable,
		Unauthorized,
		NotFound,
		ServerError,
		UnknownError,
		Undefined
	};

	internal static class MetacoErrorsDefinitions
	{
		private static readonly Dictionary<ErrorType, string> Errors = new Dictionary<ErrorType, string> {
			{ErrorType.InvalidInput, "invalid_input"},
			{ErrorType.ApiCallsQuotaExceeded, "api_calls_quota_exceeded"},
			{ErrorType.SmsSendingFailed, "sms_sending_failed"},
			{ErrorType.PhoneConfirmationNotFound, "phone_confirmation_not_found"},
			{ErrorType.InvalidConfirmationCode, "invalid_confirmation_code"},
			{ErrorType.OrderNotFound, "order_not_found"},
			{ErrorType.NotEnoughFunds, "not_enough_funds"},
			{ErrorType.OrderTooSmall, "order_too_small"},
			{ErrorType.OrderCountLimitExceeded, "order_count_limit_exceeded"},
			{ErrorType.YearlyTransactionQuotaExceeded, "yearly_transaction_quota_exceeded"},
			{ErrorType.MaximumTransactionAmountExceeded, "maximum_transaction_amount_exceeded"},
			{ErrorType.OrderNotCancellable, "order_not_cancellable"},
			{ErrorType.Unauthorized, "unauthorized"},
			{ErrorType.NotFound, "notfound"},
			{ErrorType.ServerError, "servererror"},
			{ErrorType.UnknownError, "unknownerror"},
			{ErrorType.Undefined, ""}
		};

		public static ErrorType FromString(string parameterName)
		{
			if (!string.IsNullOrEmpty(parameterName))
			{
				var kp = Errors.FirstOrDefault(e => e.Value.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));
				if (!Equals(kp, default(KeyValuePair<ErrorType, string>)))
					return kp.Key;
			}
			return ErrorType.Undefined;
		}

		public static ErrorType GetErrorType(MetacoErrorResult result)
		{
			if (result == null)
			{
				return ErrorType.UnknownError;
			}

			var type = FromString(result.MetacoError);
			if (type == ErrorType.Undefined)
			{
				if (result.Status == 404)
				{
					type = ErrorType.NotFound;
				}
				else if (result.Status == 401)
				{
					type = ErrorType.Unauthorized;
				}
				else if (result.Status >= 500 && result.Status < 600)
				{
					type = ErrorType.ServerError;
				}
				else
				{
					return ErrorType.UnknownError;
				}
			}
			return type;
		}
	}
}