﻿namespace ExchangeServiceAPI.Dtos
{
    public class ExchangeRequest
    {
        public decimal Amount { get; set; }
        public string InputCurrency { get; set; }
        public string OutputCurrency { get; set; }
    }
}