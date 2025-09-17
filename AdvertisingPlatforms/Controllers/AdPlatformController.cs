using AdvertisingPlatforms.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingPlatforms.Controllers
{
    [ApiController]
    public class AdPlatformController : Controller
    {
        private readonly IAdPlatformService adPlatformService;

        public AdPlatformController(IAdPlatformService platformService)
        {
            adPlatformService = platformService;
        }

        [HttpPost("search")]
        public IActionResult LoadPlatformsFromFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Файл не загружен");
                }

                adPlatformService.LoadFile(file);
                return Ok("Загрузка файла завершена");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Ошибка считывания файла: {ex.Message}");
            }
        }

        [HttpGet("upload")]
        public IActionResult SearchAdPlatforms(string location)
        {
            try
            {
                 var platforms = adPlatformService.FindPlatformsForLocation(location);
                 return Ok(platforms);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Ошибка при поиске рекламных платформ: {ex.Message}");
            }
            
        }
    }
}
