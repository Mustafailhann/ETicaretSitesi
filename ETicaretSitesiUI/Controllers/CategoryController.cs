using ETicaretDal.Abstract;
using ETicaretData.Context;
using ETicaretData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicaretSitesiUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ETicaretContext _contex;
        private readonly ICategoryDal _categoryDal;
        public CategoryController(ETicaretContext contex, ICategoryDal categoryDal)
        {
            _contex = contex;
            _categoryDal = categoryDal;
        }

        public IActionResult Index()
        {
            var cr = _contex.Categories.ToList();
            return View(cr);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Id", "Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryDal.Add(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            _categoryDal.Delete(new Category { Id = id });
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var category = _categoryDal.Get(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id", "Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _categoryDal.Update(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }
}
