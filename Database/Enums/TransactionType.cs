using System.ComponentModel;

namespace Database.Enums
{
    public enum TransactionType
    {
        [Description("Kup")]
        Long,
        [Description("Sprzedaj")]
        Short
    }
}