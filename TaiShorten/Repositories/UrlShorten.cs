using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaiShorten.Data;
using TaiShorten.Models;

namespace TaiShorten.Repositories
{
    public class UrlShorten : IUrlShorten
    {
        private readonly ApplicationDbContext _dbContext;

        public UrlShorten(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AccessCountAsync()
        {
            var accessCount = new Random().Next(1, 3);
            var totalCount = await _dbContext.shortUrls!.SumAsync(u => u.AccessCount);
            totalCount += accessCount;
            var urlRecord = await _dbContext.shortUrls!.FirstOrDefaultAsync();
            if (urlRecord != null)
            {
                urlRecord.AccessCount += accessCount;
                await _dbContext.SaveChangesAsync();
            }
            return totalCount;
        }

        public string GenerateShortenedUrl()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var code = new char[7];
            for (int i = 0; i < code.Length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }
            return $"https://taishorten.somee.com/{new string(code)}";
        }

        public async Task<(int TotalAccessCount, int TotalShortenedCount)> GetCount()
        {
            var totalAccessCount = await AccessCountAsync();

            var totalShortenedCount = await _dbContext.shortUrls!.SumAsync(_ => _.ShortenedCount);
            return (totalAccessCount, totalShortenedCount);
        }

        public async Task<IActionResult> Redirect(string id)
        {
            var shortUrl = await _dbContext.shortUrls!.FirstOrDefaultAsync(u => u.ShortenedUrl!.EndsWith(id));
            if (shortUrl == null)
            {
                return null!;
            }
            return await Redirect(shortUrl.OriginalUrl!);

        }

        public async Task<(int TotalAccessCount, int TotalShortenedCount, string ShortenedUrl)> ShortenUrl(string originalUrl)
        {
            var shortenedUrl = GenerateShortenedUrl();

            while (_dbContext.shortUrls!.Any(u => u.ShortenedUrl == shortenedUrl))
            {
                shortenedUrl = GenerateShortenedUrl();
            }

            var _totalAccessCount = await AccessCountAsync();

            var shortenedCount = new Random().Next(1, 3);
            var ShortenedCount = shortenedCount;

            var totalShortenedCount = _dbContext.shortUrls!.Sum(u => u.ShortenedCount);
            totalShortenedCount += shortenedCount;

            var shortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortenedUrl,
                ShortenedCount = shortenedCount
            };
            await _dbContext.shortUrls!.AddAsync(shortUrl);
            await _dbContext.SaveChangesAsync();

            var _shortenedUrl = shortenedUrl;
            var _totalShortenedCount = totalShortenedCount;

            return (_totalAccessCount, _totalShortenedCount, _shortenedUrl);
        }
    }
}
