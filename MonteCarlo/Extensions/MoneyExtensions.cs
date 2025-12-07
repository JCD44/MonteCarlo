using System;
using Narvalo;

namespace MonteCarlo.Extensions
{
    public static class MoneyExtensions
    {
        public static Money AsMoney(this int value)
        {
            return new Money(value, Currency.UnitedStatesDollar);
        }
        public static Money AsMoney(this decimal value)
        {
            return new Money(value, Currency.UnitedStatesDollar);
        }
        public static Money AsMoney(this double value)
        {
            return new Money((decimal)value, Currency.UnitedStatesDollar);
        }
        public static Money AsMoneyByCast(this object value)
        {
            return new Money(Convert.ToDecimal(value), Currency.UnitedStatesDollar);
        }


    }
}
