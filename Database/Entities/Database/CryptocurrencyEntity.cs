using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Entities.Database;

[Table("Cryptocurrency")]
[Index(nameof(Code), IsUnique = true)]
public sealed record CryptocurrencyEntity
{
    [Key] public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}