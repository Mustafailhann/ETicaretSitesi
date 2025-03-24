using ETicaretData.Helpers;
using ETicaretData.Identity;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretSitesiUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            if(User.Identity == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            var user = _userManager.FindByNameAsync(login.UserName).Result;
            if (user == null) {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                return View(login);
            }
            var result = _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, false).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("", "Home");
            }
            return View(login);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var user = new AppUser
            {
                UserName = register.UserName,
                Email = register.Email,
                Name = register.Ad,
                Surname = register.soyad
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            await _userManager.AddToRoleAsync(user, "user");

            if (result.Succeeded)
            {
                // Kullanıcıyı otomatik giriş yaptır
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Account");
            }

            // Hataları ModelState'e ekle
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(register);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var cart = SesionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "Card");
            if (cart != null) {
                HttpContext.Session.Remove("Card");
                //cart.Clear();
                //SesionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
