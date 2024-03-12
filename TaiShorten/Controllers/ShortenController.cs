﻿using Microsoft.AspNetCore.Mvc;
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
			// Đếm lượt truy cập và lưu vào database
			var accessCount = new Random().Next(1, 6);
			ViewBag.AccessCount = accessCount;

			var totalCount = _dbContext.shortUrls!.Sum(u => u.AccessCount);
			totalCount += accessCount;

			ViewBag.TotalAccessCount = totalCount;


			ViewBag.TotalShortenedCount= _dbContext.shortUrls!.Sum(_ => _.ShortenedCount);

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

			var totalCount = _dbContext.shortUrls!.Sum(u => u.AccessCount);

			ViewBag.TotalAccessCount = totalCount;

			// Đếm lượt rút gọn link và lưu vào database
			var shortenedCount = new Random().Next(1, 6);
			ViewBag.ShortenedCount = shortenedCount;

			var totalShortenedCount = _dbContext.shortUrls!.Sum(u => u.ShortenedCount);
			totalShortenedCount += shortenedCount;

			var shortUrl = new ShortUrl { OriginalUrl = originalUrl, ShortenedUrl = shortenedUrl, ShortenedCount = shortenedCount };
			_dbContext.shortUrls!.Add(shortUrl);
			_dbContext.SaveChanges();

			ViewBag.ShortenedUrl = shortenedUrl;
			ViewBag.TotalShortenedCount = totalShortenedCount;

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
			// Chuỗi ký tự mà chúng ta sẽ sử dụng để tạo mã, bao gồm chữ hoa, chữ thường và số
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

			// Tạo một đối tượng ngẫu nhiên để tạo ra giá trị ngẫu nhiên
			var random = new Random();

			// Mảng chứa mã ngắn được tạo
			var code = new char[7];

			// Lặp qua từng vị trí trong mã và gán một ký tự ngẫu nhiên từ chuỗi chars
			for (int i = 0; i < code.Length; i++)
			{
				code[i] = chars[random.Next(chars.Length)];
			}

			// Trả về đường dẫn ngắn với mã mới tạo
			return $"https://localhost:44351/{new string(code)}";
		}

	}
}
