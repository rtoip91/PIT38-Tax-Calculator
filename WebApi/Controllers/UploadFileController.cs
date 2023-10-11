using Database.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaxCalculatingService.Interfaces;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/file/[action]")]
    [ApiController]
    public sealed class UploadFileController : ControllerBase
    {
        private readonly IFileUploadHelper _fileUploadHelper;
        private readonly IFileDataAccess _fileDataAccess;
        private readonly IConfiguration _configuration;

        public UploadFileController(
            IFileUploadHelper fileUploadHelper,
            IConfiguration configuration,
            IFileDataAccess fileDataAccess)
        {
            _configuration = configuration;
            _fileUploadHelper = fileUploadHelper;
            _fileDataAccess = fileDataAccess;
        }

        [HttpPost(Name = "uploadInputFileStressTest")]
        public async Task<IActionResult> UploadFileStressTest(IFormFile inputExcelFile, int occurence,
            CancellationToken token)
        {
            for (int i = 0; i < occurence; i++)
            {
                if (!token.IsCancellationRequested)
                {
                    await _fileUploadHelper.UploadFile(inputExcelFile);
                }
            }

            return Ok("Stress Testing !!!");
        }


        /// <summary>
        /// Posts the excel input file
        /// </summary>
        /// <param name="inputExcelFile">Excel input file</param>
        /// <param name="token">Cancellation token</param>
        /// <returns>File upload result</returns>
        [HttpPost(Name = "uploadInputFile")]
        public async Task<IActionResult> UploadFile(IFormFile inputExcelFile, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Cancellation requested");
            }

            Guid? result = await _fileUploadHelper.UploadFile(inputExcelFile);

            if (result.HasValue)
            {
                return Ok(new { result });
            }

            return StatusCode(StatusCodes.Status400BadRequest, "Incorrect file to upload");
        }

        [HttpGet(Name = "getResultFile")]
        public async Task<IActionResult> GetResultFile(Guid operationId)
        {
            var filename = await _fileDataAccess.GetCalculationResultFileNameAsync(operationId);
            if (filename == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File doesn't exist");
            }

            var filePath = Path.Combine(_configuration["ResultStorageFolder"], filename);
            if (!System.IO.File.Exists(filePath))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File doesn't exist");
            }

            await using var stream = System.IO.File.OpenRead(filePath);
            await _fileDataAccess.SetAsDownloadedAsync(operationId);
            return File(stream, "application/octet-stream", filename);
        }
    }
}