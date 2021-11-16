using System;

namespace TaxEtoro.Interfaces
{
    internal interface IEventsSubscriber
    {
        void AfterCfd(object sender, EventArgs e);
        void AfterCrypto(object sender, EventArgs e);
        void AfterDividend(object sender, EventArgs e);
        void AfterStock(object sender, EventArgs e);
    }
}
