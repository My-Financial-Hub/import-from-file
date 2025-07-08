using Microsoft.AspNetCore.Mvc;
using MyFinancialHub.Application.CQRS;
using MyFinancialHub.Import.Application.Handlers.ImportPdfFile;

namespace MyFinancialHub.Import.WebApi.Controllers
{
    [ApiController]
    [Route("import-balances")]
    public class ImportBalancesController(IDispatcher dispatcher) : ControllerBase
    {
        private readonly IDispatcher dispatcher = dispatcher;

        [HttpPost("pfd/{accountName}")]
        public async Task<IActionResult> Post(
            [FromForm] IFormFile file, 
            string accountName
            )
        {
            var command = new ImportPdfFileCommand(file.OpenReadStream(), accountName);
            await this.dispatcher.Dispatch(command);

            return Created();
        }
    }
}
