using System;
using System.IO;
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

        private FileStream GetLocalFile(string fileLocation)
        {
            return new FileStream(
                fileLocation,
                FileMode.Open,
                FileAccess.Read);
        }
        
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Get(string fileName)
        {
            try
            {
                var fileStream = GetLocalFile($@"{_config["UrlBasePath"]}{fileName}");
                _logger.LogInformation($"Downloading {fileName}...");
                return File(fileStream, "application/octet-stream");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"The file: {fileName} doesn't exist.");
            }

            return NotFound();
        }
    }
}