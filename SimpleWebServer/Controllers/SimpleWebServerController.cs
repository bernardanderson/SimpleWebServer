using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SimpleWebServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class SimpleWebServerController : ControllerBase
    {
        private readonly ILogger<SimpleWebServerController> _logger;
        private readonly IConfiguration _config;
        
        public SimpleWebServerController(IConfiguration configuration, ILogger<SimpleWebServerController> logger)
        {
            _logger = logger;
            _config = configuration;
        }

        [HttpGet]
        [Route("health")]
        public IActionResult Get()
        {
            return Ok("Server is Up!");
        }

        [HttpGet]
        [HttpHead]
        [Route("{fileName}")]
        public IActionResult Get(string fileName)
        {
            try
            {
                var filePath = $"{_config["UrlBasePath"]}{fileName}";
                var fileResponse = new FileStreamResult(System.IO.File.OpenRead(filePath), "application/octet-stream")
                {
                    EnableRangeProcessing = true
                };
                _logger.LogInformation($"Downloading {fileName}...");
                return fileResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"The file: {fileName} doesn't exist.", ex);
            }

            return NotFound();
        }
    }
}