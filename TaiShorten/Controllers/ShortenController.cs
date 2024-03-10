using Microsoft.AspNetCore.Mvc;
using System;
using TaiShorten.Data;
using TaiShorten.Models;

namespace TaiShorten.Controllers
{
    public class ShortenController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ShortenController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ShortenUrl(string originalUrl)
        {
            var shortenedUrl = GenerateShortenedUrl();

            while (_dbContext.shortUrls!.Any(u => u.ShortenedUrl == shortenedUrl))
            {
                shortenedUrl = GenerateShortenedUrl();
            }

            var shortUrl = new ShortUrl { OriginalUrl = originalUrl, ShortenedUrl = shortenedUrl };
            _dbContext.shortUrls!.Add(shortUrl);
            _dbContext.SaveChanges();

            ViewBag.ShortenedUrl = shortenedUrl;
            return View("Index");
        }

        [HttpGet("{id}")]
        public new IActionResult Redirect(string id)
        {
            var shortUrl = _dbContext.shortUrls!.FirstOrDefault(u => u.ShortenedUrl!.EndsWith(id));
            if (shortUrl != null)
            {
                return base.Redirect(shortUrl.OriginalUrl!);
            }

            return NotFound();
        }


        private string GenerateShortenedUrl()
        {
            var guid = Guid.NewGuid().ToString("N").Substring(0, 6);
            return $"https://localhost:44351/{guid}";
        }
    }
}
