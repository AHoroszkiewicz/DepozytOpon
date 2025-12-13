using DepozytOpon.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepozytOpon.Controllers
{
    public class ScraperController : Controller
    {
        private readonly TireScraperService _scraper;

        public ScraperController(TireScraperService scraper)
        {
            _scraper = scraper;
        }

        [HttpPost]
        public async Task<IActionResult> Scrape(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return BadRequest(new { error = "URL nie może być pusty." });

            var data = await _scraper.ScrapeAsync(url);

            if (!data.Success)
                return Json(new { success = false, error = data.Error });

            return Json(new { success = true, data });
        }

    }
}
