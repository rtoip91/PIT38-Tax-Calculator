using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Database.Enums;
using Newtonsoft.Json.Converters;

namespace Database.Entities.Database
{
    [Table("File")]
    public class FileEntity
    {
        [Key] public int Id { get; set; }

        public string InputFileName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FileStatus Status { get; set; }

        public string CalculationResultFileName { get; set; }

        public string CalculationResultJson { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public Guid OperationGuid { get; set; }
    }
}