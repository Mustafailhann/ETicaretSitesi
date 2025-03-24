using ETicaretDal.Abstract;
using ETicaretData.Context;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ETicaretSitesiUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly ETicaretContext _contex;
        private readonly IOrderDal _orderDal;

        public OrderController(ETicaretContext contex, IOrderDal orderDal)
        {
            _contex = contex;
            _orderDal = orderDal;
        }

        public IActionResult Index()
        {
            var orders = _contex.Orders.ToList();
            

            return View(orders);
        }


        [HttpPost]
        public IActionResult Approve(int id)
        {
            var order = _contex.Orders.Find(id);
            if (order != null)
            {
                order.orderState = EnumOrderState.completed;
                _contex.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AutoReject(int id)
        {
            var order = _contex.Orders.Find(id);
            if (order != null && order.orderState == EnumOrderState.waiting)
            {
                order.orderState = EnumOrderState.waiting;
                _contex.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
