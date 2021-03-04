using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using UStore.Models;
using UStore.Models.Authorization;

namespace UStore.Controllers {
    public class AccountController : Controller {
        private StoreDBContext dbContext;

        public AccountController(StoreDBContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register() => View();

        [HttpPost]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model) {
            if (ModelState.IsValid) {
                Models.User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Phone == model.Phone);
                if (user == null) {
                    user = new User {
                        Name = model.Name,
                        Phone = model.Phone,
                        Password = model.Password,
                        Role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == RoleTypes.UserRole)
                    };
                    dbContext.Users.Add(user);
                    await dbContext.SaveChangesAsync();
                    await Authanticate(user);
                    return RedirectToAction("index", "home");
                }
                else
                    ModelState.AddModelError("", "Некорректные данные");
            }
            return View(model);
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login() => View();

        [HttpPost]
        [Route("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model) {
            if (ModelState.IsValid) {
                Models.User user = await dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Phone == model.Phone && u.Password == model.Password);
                if (user != null) {
                    await Authanticate(user);
                    return RedirectToAction("index", "home");
                }
                else
                    ModelState.AddModelError("", "Некорректные данные");
            }
            return View(model);
        }

        private async Task Authanticate(Models.User user) {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Phone),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Route("logout")]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }
    }
}
