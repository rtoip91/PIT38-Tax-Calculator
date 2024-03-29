﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Database.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

namespace Database.Entities.Database
{
    [Table("File")]
    [Index(nameof(OperationGuid), IsUnique = true)]
    public record FileEntity
    {
        [Key] public int Id { get; set; }

        public string InputFileName { get; set; }
        
        public InputFileContentEntity InputFileContent { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FileStatus Status { get; set; }

        public string CalculationResultFileName { get; set; }
        
        public ResultFileContentEntity CalculationResultFileContent { get; set; }

        public string CalculationResultJson { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public Guid OperationGuid { get; set; }

        public FileVersion FileVersion { get; set; }
    }
}