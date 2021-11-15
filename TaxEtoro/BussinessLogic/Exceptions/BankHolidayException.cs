using System;

namespace TaxEtoro.BussinessLogic
{
    internal class BankHolidayException : Exception
    {
        public BankHolidayException(string message) : base(message)
        {
        }

        public BankHolidayException() : base()
        {
        }
    }
}
