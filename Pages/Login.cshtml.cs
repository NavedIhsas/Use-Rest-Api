using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text.Unicode;
using System.Text;
using Catologs.Models;

namespace Catologs.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        public LoginModel(IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }
        
        public LoginDto Login { get; set; }
        public string ReuturnUrl { get; set; }

        [TempData]
        public bool TempMessage { get; set; } = false;
        public void OnGet(string returnUrl)
        {
        }

        public void OnPost(LoginDto login)
        {
            if (!ModelState.IsValid) return;
            var hashPassoword = _configuration["BaseConfig:loadPage"];
            var discript = Base64Decode(hashPassoword);
            if (login.Password != discript)
            {
                TempMessage = true;
                return;
            }
            else
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "کیت گروپ"),
                new Claim(ClaimTypes.Role, ""),
               new Claim(ClaimTypes.Hash, hashPassoword)
        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                    IsPersistent = false,
                };

                _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }

        }

        public IActionResult OnGetSignOut()
        {
            _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
