using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Models;

namespace HotelBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ===== ĐĂNG KÝ =====
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName ?? ""
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
            return View();
        }

        // ===== ĐĂNG NHẬP =====
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập email và mật khẩu";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                email, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                var roles = await _userManager.GetRolesAsync(user!);

                if (roles.Contains("Admin"))
                    return RedirectToAction("Dashboard", "Admin");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email hoặc mật khẩu không đúng";
            return View();
        }

        // ===== ĐĂNG XUẤT =====
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); 
            return RedirectToAction("Index", "Home");
        }

        // ===== NÂNG QUYỀN ADMIN (xoá sau khi dùng) =====
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MakeAdmin(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Content("Thiếu email");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Content("Không tìm thấy user: " + email);

            var already = await _userManager.IsInRoleAsync(user, "Admin");
            if (already)
                return Content(email + " đã là Admin rồi!");

            await _userManager.AddToRoleAsync(user, "Admin");
            return Content($"Xong! {email} đã là Admin. Hãy đăng xuất rồi đăng nhập lại.");
        }
    }
}