
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using TaiShorten.Data;
using TaiShorten.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUrlShorten, UrlShorten>();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; // Cho phép nén cả khi sử dụng HTTPS
    options.Providers.Add<GzipCompressionProvider>(); // Sử dụng Gzip để nén
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" }); // Cấu hình các loại MIME nén
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseResponseCompression(); // middleware nén

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shorten}/{action=Index}/{id?}");
app.Run();
