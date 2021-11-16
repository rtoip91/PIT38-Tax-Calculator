using System;
using TaxEtoro.Interfaces;

namespace TaxEtoro.BussinessLogic
{
    internal class EventsSubscriber : IEventsSubscriber
    {
        public void AfterCfd(object sender, EventArgs e)
        {
            Console.WriteLine("Zakończono obliczanie CFD");
        }

        public void AfterCrypto(object sender, EventArgs e)
        {
            Console.WriteLine("Zakończono obliczanie Kryptowalut");
        }

        public void AfterDividend(object sender, EventArgs e)
        {
            Console.WriteLine("Zakończono obliczanie Dywidend");
        }

        public void AfterStock(object sender, EventArgs e)
        {
            Console.WriteLine("Zakończono obliczanie Akcji");
        }
    }
}
