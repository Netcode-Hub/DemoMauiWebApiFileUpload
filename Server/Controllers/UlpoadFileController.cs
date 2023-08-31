using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UlpoadFileController : ControllerBase
    {
        private readonly ILogger<UlpoadFileController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UlpoadFileController(ILogger<UlpoadFileController> logger, IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                // Store the content.
                var httpContent = HttpContext.Request;

                //check for null
                if (httpContent is null)
                    return BadRequest();

                // check if the context contains multiple file.
                if (httpContent.Form.Files.Count > 0)
                {
                    // loop through
                    foreach (var file in httpContent.Form.Files)
                    {
                        // get file path 
                        var filePath = Path.Combine(webHostEnvironment.ContentRootPath, "ImagesUploadFolder");

                        // check if director exist; if NO then create.
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);

                        //copy the file to the folder
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            System.IO.File.WriteAllBytes(Path.Combine(filePath, file.FileName), memoryStream.ToArray());
                        }
                    }
                    return Ok(httpContent.Form.Files.Count.ToString() + " file(s) uploaded");
                }
                return BadRequest("No file selected");

            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new StatusCodeResult(500);
            }
           


        }
    }
}
