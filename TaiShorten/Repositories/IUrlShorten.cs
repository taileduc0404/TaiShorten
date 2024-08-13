using Microsoft.AspNetCore.Mvc;

namespace TaiShorten.Repositories
{
	public interface IUrlShorten
	{
		Task<(int TotalAccessCount, int TotalShortenedCount)> GetCount();
		Task<(int TotalAccessCount, int TotalShortenedCount, string ShortenedUrl)> ShortenUrl(string originalUrl);
		Task<IActionResult> Redirect(string id);
		string GenerateShortenedUrl();
		Task<int> AccessCountAsync();
	}
}
