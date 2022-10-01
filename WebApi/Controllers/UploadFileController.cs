using Database.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("api/file/[action]")]
    [ApiController]
    public sealed class UploadFileController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileDataAccess _fileDataAccess;
        private readonly ILogger<UploadFileController> _logger;
        private List<Task<IActionResult>> _tasks;

        public UploadFileController(IConfiguration configuration,
            IFileDataAccess fileDataAccess,
            ILogger<UploadFileController> logger)
        {
            _configuration = configuration;
            _fileDataAccess = fileDataAccess;
            _logger = logger;
            _tasks = new List<Task<IActionResult>>();
        }

       

        [HttpPost(Name = "uploadInputFileStressTest")]
        public async  Task<IActionResult> UploadFileStressTest(IFormFile inputExcelFile, int occurence)
        {

            for (int i = 0; i < occurence; i++)
            {
                await UploadFile(inputExcelFile);
            }

            return Ok("Stress Testing !!!");
        }



        /// <summary>
            /// Posts the excel input file
            /// </summary>
            /// <param name="inputExcelFile">Excel input file</param>
            /// <returns>File upload result</returns>
            [HttpPost(Name = "uploadInputFile")]
        public async Task<IActionResult> UploadFile(IFormFile inputExcelFile)
        {
            long size = inputExcelFile.Length;

            if (inputExcelFile.Length > 0)
            {
                var guid = Guid.NewGuid();
                string filename = await _fileDataAccess.AddNewFile(guid);

                var filePath = Path.Combine(_configuration["InputFileStorageFolder"],
                    filename);

                await using (var stream = System.IO.File.Create(filePath))
                {
                    await inputExcelFile.CopyToAsync(stream);
                }

                _logger.LogInformation($"Succesfuly uploaded a file {filename}");

                return Ok(new { guid });
            }

            _logger.LogWarning("Wrong file provided");
            return StatusCode(StatusCodes.Status400BadRequest, "Incorrect file to upload");
        }

        [HttpGet(Name = "getResultFile")]
        public async Task<IActionResult> GetResultFile(Guid operationId)
        {
            var filename = await _fileDataAccess.GetCalculationResultFileName(operationId);
            if(filename == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File doesn't exist");
            }

            var filePath = Path.Combine(_configuration["ResultStorageFolder"], filename);
            if (!System.IO.File.Exists(filePath))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File doesn't exist");
            }

            await using var stream = System.IO.File.OpenRead(filePath);
            await _fileDataAccess.SetAsDownloaded(operationId);
            return File(stream, "application/octet-stream", filename);            
        }
    }
}