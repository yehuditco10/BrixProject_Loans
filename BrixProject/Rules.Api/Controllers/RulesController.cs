using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rules.Services;

namespace Rules.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IRuleService _ruleService;

        public RulesController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }
        [HttpPost("{providerId}")]
        public async Task<IActionResult> CreateRulesAsync(IFormFile file, [FromRoute]int providerId)
        {
            await _ruleService.CreateRulesAsync(file,providerId);
            return Ok();
        }
    }
}