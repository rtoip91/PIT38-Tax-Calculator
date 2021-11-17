
namespace Calculations.Interfaces
{
    public interface IEventsSubscriber
    {
        void AfterCfd(object? sender, EventArgs e);
        void AfterCrypto(object? sender, EventArgs e);
        void AfterDividend(object? sender, EventArgs e);
        void AfterStock(object? sender, EventArgs e);
    }
}
