using Database.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaxEtoro.Interfaces;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/file/[action]")]
    [ApiController]
    public sealed class UploadFileController : ControllerBase
    {
        private readonly ILogger<UploadFileController> _logger;
        private readonly IFileUploadHelper _fileUploadHelper;
        private readonly IFileDataAccess _fileDataAccess;
        private readonly IConfiguration _configuration;

        public UploadFileController(IFileProcessor fileProcessor,
            IFileUploadHelper fileUploadHelper,
            IConfiguration configuration,
            IFileDataAccess fileDataAccess,
            ILogger<UploadFileController> logger)
        {
            _configuration = configuration;
            _fileUploadHelper = fileUploadHelper;
            _fileDataAccess = fileDataAccess;
            _logger = logger;
            _fileUploadHelper.Subscribe(fileProcessor);

        }

       

        [HttpPost(Name = "uploadInputFileStressTest")]
        public async  Task<IActionResult> UploadFileStressTest(IFormFile inputExcelFile, int occurence)
        {

            for (int i = 0; i < occurence; i++)
            {
                await _fileUploadHelper.UploadFile(inputExcelFile);
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
            Guid? result = await _fileUploadHelper.UploadFile(inputExcelFile);

            if (result.HasValue)
            {
                return Ok(new { result });
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