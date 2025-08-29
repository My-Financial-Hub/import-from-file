using Microsoft.AspNetCore.Mvc;
using MyFinancialHub.Application.CQRS;
using MyFinancialHub.Import.Application.Handlers.ImportPdfFile;

namespace MyFinancialHub.Import.WebApi.Controllers
{
    [ApiController]
    [Route("api/import-balances")]
    public class ImportBalancesController(IDispatcher dispatcher) : ControllerBase
    {
        private readonly IDispatcher dispatcher = dispatcher;

        [HttpPost("pdf/{accountName}")]
        public async Task<IActionResult> Post(
            [FromRoute] string accountName,
            [FromForm] UploadModel form
            )
        {
            var command = new ImportPdfFileCommand(form.File.OpenReadStream(), accountName);
            await this.dispatcher.Dispatch(command);
            return Created();
        }
    }

    public record class UploadModel(IFormFile File);
}
