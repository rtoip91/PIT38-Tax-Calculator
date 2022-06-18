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
        public async Task<bool> Get()
        {
            await _actionPerformer.PerformCalculationsAndWriteResults();
            return true;
        }
    }
}
