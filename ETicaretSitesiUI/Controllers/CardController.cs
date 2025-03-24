using ETicaretDal.Abstract;
using ETicaretData.Entities;
using ETicaretData.Helpers;
using ETicaretData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ETicaretSitesiUI.Controllers
{
    public class CardController : Controller
    {
        private readonly IOrderDal _orderDal;
        private readonly IProductDal _productDal;

        public CardController(IOrderDal orderDal, IProductDal productDal)
        {
            _orderDal = orderDal;
            _productDal = productDal;
        }
        public IActionResult Index()
        {
            var card = SesionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "Card");
            if(card == null)
            {
                return View();
            }
            ViewBag.Total = card.Sum(x =>x.Product.Price*x.Quantity).ToString("c");
            SesionHelper.Count = card.Count;
            return View(card);
        }

        public IActionResult Buy(int id)
        {
            if(SesionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "Card") == null)
            {
                var cart = new List<CardItem>();
                cart.Add(new CardItem { Product = _productDal.Get(id), Quantity = 1 });
                SesionHelper.SetObjectAsJson(HttpContext.Session, "Card", cart);
                
            }
            else
            {
                var cart = SesionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "Card");
                int index = isExits(cart, id);
                if (index <0)
                {
                    cart.Add(new CardItem { Product = _productDal.Get(id), Quantity = 1 });
                }
                else
                {
                    cart[index].Quantity++;

                }
                SesionHelper.SetObjectAsJson(HttpContext.Session, "Card", cart);
            }
                return RedirectToAction("Index");
        }
        private int isExits(List<CardItem> cart, int id)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id == id)
                {
                    return i;
                }
                
            }
            return -1;
        }
        [HttpGet]
        public ActionResult CheckOut()
        {

            return View(new ShippingDetails());
        }
        [HttpPost]
        public ActionResult CheckOut(ShippingDetails details)
        {
            var Cart = SesionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "Card");
            if(Cart == null)
            {
                ModelState.AddModelError("404", "sepette ürün bulunmamaktadır");

            }
            if (ModelState.IsValid)
            {
                //Cart.Clear();
                //SesionHelper.SetObjectAsJson(HttpContext.Session, "Card", Cart);
                SaveOrder(Cart, details);
                HttpContext.Session.Remove("Card");
                return RedirectToAction("Index","Card");
            }
            return View(details);
        }

        private void SaveOrder(List<CardItem>? cart, ShippingDetails details)
        {
            // Harfleri üretmek için string tanımlıyoruz
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();

            // 3 harfli rastgele bir bölüm oluşturuyoruz
            string randomLetters = new string(Enumerable.Range(0, 3)
                .Select(_ => letters[random.Next(letters.Length)])
                .ToArray());

            // 11 haneli rastgele bir sayı oluşturuyoruz
            string randomNumbers = random.NextInt64(1111111111111111, 9999999999999999).ToString();

            // Harfleri ve sayıları birleştiriyoruz
            var orderNumber = randomLetters + randomNumbers;

            var order = new Order();
            order.OrderNumber = orderNumber;
            //order.OrderNumber ="A" + (new Random().Next(111111, 999999)).ToString();
            order.Total = cart.Sum(x => x.Product.Price * x.Quantity);
            order.OrderDate = DateTime.Now;
            order.orderState = EnumOrderState.waiting;
            order.Address = details.Adres;
            order.AddressTitle = details.AdresTitle;
            order.City = details.City;
            order.UserName = details.UserName;

            order.OrderLines = new List<OrderLine>();

            foreach (var item in cart)
            {
                var orderline = new OrderLine();

                orderline.Quantity = item.Quantity;
                orderline.Price = item.Product.Price * item.Quantity;
                orderline.ProductId = item.Product.Id;
                order.OrderLines.Add(orderline);
            }
            _orderDal.Add(order);
        }
        public IActionResult Remove(int id)
        {
            var cart = SesionHelper.GetObjectFromJson<List<CardItem>>(HttpContext.Session, "Card");
            int index = isExits(cart, id);
            cart.RemoveAt(index);
            SesionHelper.SetObjectAsJson(HttpContext.Session, "Card", cart);
            return RedirectToAction("Index");
        }
    }
}
