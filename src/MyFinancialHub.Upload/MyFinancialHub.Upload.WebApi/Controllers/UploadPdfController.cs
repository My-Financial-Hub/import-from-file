using Microsoft.AspNetCore.Mvc;
using MyFinancialHub.Application.CQRS;
using MyFinancialHub.Upload.Application.Handlers.UploadPdfFile;
using MyFinancialHub.Upload.WebApi.Extensions;

namespace MyFinancialHub.Upload.WebApi.Controllers
{
    [Route("api/upload-pdf")]
    [ApiController]
    public class UploadPdfController(
        IDispatcher dispatcher
    ) : ControllerBase
    {
        private readonly IDispatcher dispatcher = dispatcher;

        [HttpPost]
        public async Task<IActionResult> UploadPdfFile(
            [FromForm] IFormFile pdfFile, 
            [FromForm] string accountName
        )
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return BadRequest("No file uploaded.");
            if (!pdfFile.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only PDF files are allowed.");
            if(pdfFile.Length > 50 * Math.Pow(1024, 2)) 
                return BadRequest("File size exceeds the 50 MB limit.");
            if (string.IsNullOrWhiteSpace(pdfFile.ContentType) || !pdfFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Invalid file type. Please upload a valid PDF file.");
            if (!pdfFile.OpenReadStream().IsPdf())
                return BadRequest("The uploaded file is not a valid PDF.");
            if (string.IsNullOrWhiteSpace(accountName))
                return BadRequest("Account name is required.");
            
            await dispatcher.Dispatch(
                new UploadPdfFileCommand(
                    pdfFile.OpenReadStream(), 
                    accountName
                )
            );

            return Created();
        }
    }
}
