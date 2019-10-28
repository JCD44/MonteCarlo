using System;
using System.Collections.Generic;
using System.Text;

namespace MonteCarlo.Extensions
{
    public static class MoneyExtensions
    {
        public static Narvalo.Money AsMoney(this int value)
        {
            return new Narvalo.Money(value, Narvalo.Currency.UnitedStatesDollar);
        }
        public static Narvalo.Money AsMoney(this decimal value)
        {
            return new Narvalo.Money(value, Narvalo.Currency.UnitedStatesDollar);
        }
        public static Narvalo.Money AsMoney(this double value)
        {
            return new Narvalo.Money((decimal)value, Narvalo.Currency.UnitedStatesDollar);
        }
        public static Narvalo.Money AsMoneyByCast(this object value)
        {
            return new Narvalo.Money(Convert.ToDecimal(value), Narvalo.Currency.UnitedStatesDollar);
        }


    }
}
