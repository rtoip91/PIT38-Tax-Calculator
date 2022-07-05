using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelReader.Dto;

namespace ExcelReader.Interfaces
{
    public interface IExcelFileHandler
    {
        Task<ExtractedDataDto> ExtractDataFromExcel(string directory, string fileName);
    }
}
