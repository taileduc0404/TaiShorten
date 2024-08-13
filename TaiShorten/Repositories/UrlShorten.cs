using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaiShorten.Data;
using TaiShorten.Models;

namespace TaiShorten.Repositories
{
	public class UrlShorten : IUrlShorten
	{
		private readonly ApplicationDbContext _dbContext;
		private static readonly Random _random = new Random();

		public UrlShorten(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<int> AccessCountAsync()
		{
			var accessCount = _random.Next(1, 3);

			// Tính tổng số lượt truy cập
			var totalCount = await _dbContext.shortUrls!.SumAsync(u => u.AccessCount) + accessCount;

			// Cập nhật số lượng truy cập cho tất cả các bản ghi
			await _dbContext.Database.ExecuteSqlRawAsync(
				"UPDATE ShortUrls SET AccessCount = AccessCount + {0}",
				accessCount);

			return totalCount;
		}

		public string GenerateShortenedUrl()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var code = new char[7];
			for (int i = 0; i < code.Length; i++)
			{
				code[i] = chars[_random.Next(chars.Length)];
			}
			return $"https://taishorten.somee.com/{new string(code)}";
		}

		public async Task<(int TotalAccessCount, int TotalShortenedCount)> GetCount()
		{
			var totalAccessCount = await AccessCountAsync();
			var totalShortenedCount = await _dbContext.shortUrls!.SumAsync(u => u.ShortenedCount);
			return (totalAccessCount, totalShortenedCount);
		}

		public async Task<IActionResult> Redirect(string id)
		{
			var shortUrl = await _dbContext.shortUrls!.FirstOrDefaultAsync(u => u.ShortenedUrl!.EndsWith(id));
			if (shortUrl == null)
			{
				return new NotFoundResult();
			}
			return new RedirectResult(shortUrl.OriginalUrl!);
		}

		public async Task<(int TotalAccessCount, int TotalShortenedCount, string ShortenedUrl)> ShortenUrl(string originalUrl)
		{
			var shortenedUrl = GenerateShortenedUrl();

			// Kiểm tra xem Url có bị trùng lặp khôngg
			while (await _dbContext.shortUrls!.AnyAsync(u => u.ShortenedUrl == shortenedUrl))
			{
				shortenedUrl = GenerateShortenedUrl();
			}

			var _totalAccessCount = await AccessCountAsync();

			var shortenedCount = _random.Next(1, 3);
			var totalShortenedCount = await _dbContext.shortUrls!.SumAsync(u => u.ShortenedCount) + shortenedCount;

			var shortUrl = new ShortUrl
			{
				OriginalUrl = originalUrl,
				ShortenedUrl = shortenedUrl,
				ShortenedCount = shortenedCount
			};

			// Thêm và lưu thay đổi trong 1 transaction
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				await _dbContext.shortUrls!.AddAsync(shortUrl);
				await _dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}

			return (_totalAccessCount, totalShortenedCount, shortenedUrl);
		}
	}
}
