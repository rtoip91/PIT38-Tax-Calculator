using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxEtoro.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalculationsController : ControllerBase
    {
        private readonly IActionPerformer _actionPerformer;
        private readonly IConfiguration _configuration;


        public CalculationsController(IActionPerformer actionPerformer,
            IConfiguration configuration)
        {
            _actionPerformer = actionPerformer;
            _configuration = configuration;
        }

        /// <summary>
        /// Posts the excel input file
        /// </summary>
        /// <param name="inputExcelFile">Excel input file</param>
        /// <returns>File upload result</returns>
        [HttpPost(Name = "PostFile")]
        public async Task<IActionResult> PostFile(IFormFile inputExcelFile)
        {
            long size = inputExcelFile.Length;

            if (inputExcelFile.Length > 0)
            {
                string filename = $"{Guid.NewGuid()}.xlsx";
                var filePath = Path.Combine(_configuration["InputFileStorageFolder"],
                    filename);

                await using (var stream = System.IO.File.Create(filePath))
                {
                    await inputExcelFile.CopyToAsync(stream);
                }

                return Ok(new { filename, size });
            }


            return StatusCode(StatusCodes.Status400BadRequest, "Incorrect file to upload");
        }
    }
}