using Database.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/file/[action]")]
    [ApiController]
    public sealed class UploadFileController : ControllerBase
    {
        private readonly IFileUploadHelper _fileUploadHelper;
        private readonly IFileDataAccess _fileDataAccess;

        public UploadFileController(
            IFileUploadHelper fileUploadHelper,
            IFileDataAccess fileDataAccess)
        {
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

            using var resultFileContent = await _fileDataAccess.GetCalculationResultFileContentAsync(operationId);

            await _fileDataAccess.SetAsDownloadedAsync(operationId);
            return File(resultFileContent, "application/octet-stream", filename);
        }
    }
}