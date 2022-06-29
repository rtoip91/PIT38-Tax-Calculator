using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Enums;

namespace Database.Entities.Database
{
    [Table("File")]
    public class FileEntity
    {
        [Key] public int Id { get; set; }
        public string InputFileName { get; set; }
        public FileStatus Status { get; set; }
        public string CalculationResultFileName { get; set; }
        public string CalculationResultJson { get; set; }
    }
}
