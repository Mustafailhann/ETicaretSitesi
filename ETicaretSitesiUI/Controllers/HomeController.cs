using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ETicaretSitesiUI.Models;
using ETicaretData.ViewModels;
using ETicaretDal.Abstract;
using ETicaretData.Identity;
using Microsoft.AspNetCore.Identity;

namespace ETicaretSitesiUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoryDal _categoryDal;
    private readonly IProductDal _productDal;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public HomeController(ILogger<HomeController> logger, ICategoryDal categoryDal, IProductDal productDal, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _logger = logger;
        _categoryDal = categoryDal;
        _productDal = productDal;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    public IActionResult Index()
    {
        var product = _productDal.GetAll(p => p.IsHome && p.IsApproved);
        return View(product);
    }

    public async Task<IActionResult> List(int? id)
    {
        ViewBag.ıd = id;
        var user = await _userManager.GetUserAsync(User);  // Mevcut kullanıcıyı al
        var product = _productDal.GetAll(x => x.IsApproved);
        if (id != null)
        {
            product = product.Where(x => x.CategoryId == id).ToList();
        }
        var models = new ListViewModel()
        {
            Categories = _categoryDal.GetAll(),
            Products = product,
            UserName = user != null ? user.UserName : "Misafir"

        };
        return View(models);
    }

    public IActionResult Details(int? id)
    {
       var pr = _productDal.Get(Convert.ToInt32(id));
        return View(pr);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


