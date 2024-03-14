using Microsoft.AspNetCore.Mvc;

namespace TaiShorten.Controllers
{
    public class LegalController : Controller
    {
        [HttpGet("TermsOfUse")]
        public IActionResult TermsOfUse()
        {
            return View();
        }

        [HttpGet("PrivacyPolicy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
