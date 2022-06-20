using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxEtoro.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationsController : ControllerBase
    {
        private readonly IActionPerformer _actionPerformer;

        public CalculationsController(IActionPerformer actionPerformer)
        {
            _actionPerformer = actionPerformer;
        }

        [HttpGet(Name = "RunCalculations")]
        public async Task<string> Get()
        {
            var timer = new Stopwatch();

            timer.Start();


            await _actionPerformer.PerformCalculationsAndWriteResults();

            timer.Stop();

            TimeSpan timeTaken = timer.Elapsed;
           
            return $"Time taken: {timeTaken:m\\:ss\\.fff}";
        }
    }
}
