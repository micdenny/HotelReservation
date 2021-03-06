﻿namespace Payments.ViewModelComposition.Model
{
    internal class ReservationPaymentDetailsModel
    {
        public string ReservationId { get; set; }
        public string CustomerId { get; set; }
        public string CardNumber { get; set; }
        public string ExpieryDateMonth { get; set; }
        public string ExpieryDateYear { get; set; }
        public string CCV { get; set; }
        public string PaymentAmount { get; set; }
    }
}
