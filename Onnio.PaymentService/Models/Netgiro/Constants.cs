using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.PaymentService.Models.Netgiro
{
    public static class NetgiroConstants
    {
        //MAke this list of constans consistent with the Netgiro API documentation
        public const int PaymentCompleted = 10200;
        public const int PaymentCanceled = 10201;
        public const int PaymentAlreadyCanceled = 10202;
        public const int PaymentAlreadyConfirmed = 10203;

        // Customer and Cart Related Codes
        public const int CartNotValid = 10304;
        public const int MinimumAmountError = 10305;
        public const int CustomerDoesNotExist = 10306;

        // Additional Customer Declined Payment Codes
        //Make this a dictionary or enum if more codes are added

        public const int CustomerDeclinedPayment = 10302;
        public const int CustomerDeclinedPayment2 = 10307;
        public const int CustomerDeclinedPayment3 = 10422;
        public const int CustomerDeclinedPayment4 = 10423;

        // Cart and Reservation Status Codes
        public const int CartNotFound = 10424; // Note: Same code as CustomerDeclinedPayment5
        public const int PendingCustomerPaymentConfirmation = 10425;
        public const int ReservationCreatedWaitingProviderConfirmation = 10426;
        public const int ConfirmationTypeNotValid = 10427;
        public const int AdditionalConfirmationNeeded = 10428;
        public const int ReservationCreatedWaitingProviderCallback = 10429;

        public static string GetDescription(int statusCode)
        {
            return statusCode switch
            {
                PaymentCompleted => "Payment completed",
                PaymentCanceled => "Payment canceled",
                PaymentAlreadyCanceled => "Payment already canceled",
                PaymentAlreadyConfirmed => "Payment already confirmed",
                CustomerDeclinedPayment => "Customer declined payment",
                CartNotValid => "Cart not valid",
                MinimumAmountError => "Minimum amount error",
                CustomerDoesNotExist => "Customer does not exist",
                CustomerDeclinedPayment2 => "Customer declined payment",
                CustomerDeclinedPayment3 => "Customer declined payment",
                CustomerDeclinedPayment4 => "Customer declined payment",
                CartNotFound => "Cart not found",
                PendingCustomerPaymentConfirmation => "Pending customer payment confirmation (to confirm payment request in mobile app)",
                ReservationCreatedWaitingProviderConfirmation => "Reservation created and waiting for provider confirmation (when ConfirmationType = Manual, provider needs to call ConfirmCart)",
                ConfirmationTypeNotValid => "Confirmation type not valid (when calling InsertCart)",
                AdditionalConfirmationNeeded => "Additional confirmation needed (in cases when after SSN, provider needs to enter SMS code from customer)",
                ReservationCreatedWaitingProviderCallback => "Reservation created and waiting for provider callback response (when ConfirmationType = ServerCallback)",
                _ => "Unknown status code"
            };
        }
    }
}
