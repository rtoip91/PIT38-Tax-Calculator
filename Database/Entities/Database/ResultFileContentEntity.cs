using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.Database;

[Table("ResultFileContent")]
public record ResultFileContentEntity
{
    [Key] public int Id { get; set; }
    public byte[] FileContent { get; set; }
}