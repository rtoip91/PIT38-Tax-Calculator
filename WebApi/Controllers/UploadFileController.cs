using Database.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/file/[action]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileDataAccess _fileDataAccess;

        public UploadFileController(IConfiguration configuration,
            IFileDataAccess fileDataAccess)
        {
            _configuration = configuration;
            _fileDataAccess = fileDataAccess;
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

                return Ok(new { guid });
            }


            return StatusCode(StatusCodes.Status400BadRequest, "Incorrect file to upload");
        }
    }
}