using ETicaretData.Identity;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretSitesiUI.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin"); // Admin olanları getir
            var adminIds = admins.Select(a => a.Id).ToList(); // Admin ID'lerini listeye çevir
            var users = _userManager.Users.Where(u => !adminIds.Contains(u.Id)).ToList(); // Admin olmayanları getir
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> RoleAssign(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()); // belirtilen id ye sahip kullanıcı bulur.
            var roles = _roleManager.Roles.Where(i => i.Name != "Admin").ToList();//admin olmayan rolleri listeleme
            var userRoles = await _userManager.GetRolesAsync(user);  //kullanıcının mevcut rollerini getireceğiz ahmete ait tüm roller mehmete ait tüm roller.
            var RoleAssings = new List<RoleAssingModels>(); //kullanıcıya atanacak olan rollerin listesi

            roles.ForEach(role => RoleAssings.Add(new RoleAssingModels
            {
                HasAssing = userRoles.Contains(role.Name),//Kullanıcının bu role sahip olup olmadığını belirler.
                Id = role.Id,
                Name = role.Name
            }));
            ViewBag.name = user.Name; // kullanıcının adını frontende gönderir
            return View(RoleAssings); //hazırlanan rol listesini view e gönderir
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssingModels> models, int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var userRoles = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                return NotFound();
            }

            // _userManager.AddToRoleAsync(user, "RoleName");
            foreach (var role in models)
            {
                if (role.HasAssing && !userRoles.Contains(role.Name))
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!role.HasAssing && userRoles.Contains(role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }

            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var sonuc = await _userManager.DeleteAsync(user);
            if(sonuc.Succeeded)
            {
               return RedirectToAction("Index");
            }
            return NotFound("silme işlemi başarısız");
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Şu an giriş yapan kullanıcının bilgilerini getiriyoruz
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                ViewBag.Message = "Bilgiler başarıyla güncellendi.";
                return View(model);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }


    }
}
