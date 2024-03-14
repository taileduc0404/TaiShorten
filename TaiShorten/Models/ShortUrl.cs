namespace TaiShorten.Models
{
	public class ShortUrl
	{
		public int Id { get; set; }
		public string? OriginalUrl { get; set; }
		public string? ShortenedUrl { get; set; }
		public int AccessCount { get; set; }
		public int ShortenedCount { get; set; }
	}
}
