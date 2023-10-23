using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Enums;

namespace Database.Entities.Database;

[Table("FileContent")]
public record FileContentEntity
{
    [Key] public int Id { get; set; }
    
    public FileType FileType { get; set; }
    
    public byte[] FileContent { get; set; }
}