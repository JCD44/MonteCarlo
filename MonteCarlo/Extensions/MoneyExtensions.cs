using System;
using NodaMoney;

namespace MonteCarlo.Extensions
{
    public static class MoneyExtensions
    {

        public static Currency DefaultCurrency { get {  return Currency.FromCode("USD"); } }
        public static Money AsMoney(this int value)
        {
            return new Money(value, DefaultCurrency, MidpointRounding.AwayFromZero);
        }
        public static Money AsMoney(this decimal value)
        {
            return new Money(value, DefaultCurrency, MidpointRounding.AwayFromZero);
        }
        public static Money AsMoney(this double value)
        {
            return new Money((decimal)value, DefaultCurrency, MidpointRounding.AwayFromZero);
        }
        public static Money AsMoneyByCast(this object value)
        {
            return new Money(Convert.ToDecimal(value), DefaultCurrency, MidpointRounding.AwayFromZero);
        }


    }
}
