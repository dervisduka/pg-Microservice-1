using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        public VersionController(IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetVersion()
        {
            var versionFilePath = Path.Combine(_hostEnvironment.ContentRootPath, "version.json");

            if (!System.IO.File.Exists(versionFilePath))
            {
                return NotFound("Version file not found.");
            }

            var versionJson = await System.IO.File.ReadAllTextAsync(versionFilePath);
            var versionData = JObject.Parse(versionJson);

            var version = versionData["version"]?.ToString();

            var response = new
            {
                MicroserviceName = _configuration["MicroserviceName"],
                Version = version
            };

            return Ok(response);
        }
    }
}
