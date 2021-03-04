using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using UStore.Models;

namespace UStore.Controllers {
    public class HomeController : Controller {
        StoreDBContext dbContext;

        public HomeController(StoreDBContext dbContext) {
            this.dbContext = dbContext;
        }

        [Route("~/")]
        public IActionResult Index() {
            return View();
        }

        [Route("products")]
        [Authorize]
        public IActionResult Products() {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            if(role == RoleTypes.AdminRole) {
                var productList = dbContext.Products.ToList();
                return View("ProductsAdmin", productList);
            }
            else if(role == RoleTypes.UserRole) {
                var productList = dbContext.Products.Where(p => p.CountInStock > 0).ToList();
                return View("ProductsUser", productList);
            }
            return View("MessageView", "В доступе отказано");
        }

        [Route("buy")]
        [Authorize(Roles = RoleTypes.UserRole)]
        public IActionResult Buy(int productId) {
            Product product = dbContext.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null) {
                string phone = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
                Models.User user = dbContext.Users.First(u => u.Phone == phone);
                Order order = new Order() {
                    Date = DateTime.Now,
                    UserId = user.Id,
                    ProductId = product.Id
                };
                dbContext.Orders.Add(order);

                product.CountInStock -= 1;
                dbContext.Products.Update(product);

                dbContext.SaveChanges();
                return View("MessageView", $"Спасибо за покупку, {user.Name}");
            }
            else
                return View("MessageView", "Такого товара нет");
        }

        [Route("updateproduct")]
        [Authorize(Roles = RoleTypes.AdminRole)]
        [HttpGet]
        public IActionResult UpdateProduct(int productId = 0) {
            ViewBag.ProductId = productId;
            if (productId == 0)
                return View();
            else {
                Product product = dbContext.Products.First(p => p.Id == productId);
                return View(product);
            }
        }

        [Route("updateproduct")]
        [Authorize(Roles = RoleTypes.AdminRole)]
        [HttpPost]
        public IActionResult UpdateProduct(Product product) {
            if (ModelState.IsValid) {
                if (ViewBag.ProductId == 0)
                    dbContext.Products.Add(product);
                else
                    dbContext.Products.Update(product);

                dbContext.SaveChanges();
                return RedirectToAction("products");
            }
            return View(product);
        }

        [Route("orders")]
        [Authorize(Roles = RoleTypes.AdminRole)]
        public IActionResult Orders() {
            var orders = from order in dbContext.Orders
                         join user in dbContext.Users on order.UserId equals user.Id
                         join product in dbContext.Products on order.ProductId equals product.Id
                         select new OrdersModel {
                             UserName = user.Name,
                             Product = product.Name,
                             Manufacturer = product.Manufacturer,
                             Price = product.Price,
                             Date = order.Date
                         };

            return View(orders);
        }
    }
}
