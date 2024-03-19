using Microsoft.AspNetCore.Mvc;
using TaiShorten.Data;
using TaiShorten.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaiShorten.Repositories;

namespace TaiShorten.Controllers
{
    public class ShortenController : Controller
    {
        private readonly IUrlShorten _urlShorten;

        public ShortenController(IUrlShorten urlShorten)
        {
            _urlShorten = urlShorten;
        }

        public async Task<IActionResult> Index()
        {
            var countResult = await _urlShorten.GetCount();
            ViewBag.TotalAccessCount = countResult.TotalAccessCount;

            ViewBag.TotalShortenedCount = countResult.TotalShortenedCount;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShortenUrl(string originalUrl)
        {
            var urlResult = await _urlShorten.ShortenUrl(originalUrl);
            ViewBag.TotalAccessCount = urlResult.TotalAccessCount;
            ViewBag.ShortenedUrl = urlResult.ShortenedUrl;
            ViewBag.TotalShortenedCount = urlResult.TotalShortenedCount;
            return View("Index");
        }

        [HttpGet("{id}")]
        public new async Task<IActionResult> Redirect(string id)
        {
            var result=  await _urlShorten.Redirect(id);
            if (result is null)
            {
                return NotFound();
            }
            return result;
        }

        private string GenerateShortenedUrl()
        {
            return _urlShorten.GenerateShortenedUrl();
        }

        private async Task AccessCountAsync()
        {
            await _urlShorten.AccessCountAsync();
        }
    }
}
