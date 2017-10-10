using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KraudFan2_0.Models;
using KraudFan2_0.Data;
using Microsoft.AspNetCore.Identity;
using KraudFan2_0.Models.AppData;
using Microsoft.AspNetCore.Authorization;
namespace KraudFan2_0.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        public async Task< IActionResult> Index()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            return View(users);
        }
        public IActionResult Projects()
        {
            return View();
        }
        public IActionResult Achievment()
        {
            return View();
        }

        public async Task< ActionResult> Manage(string[] Ids, string action)
        {
            ViewBag.Id = Ids;
            if (action == "ban")
            {
                return View("Ban");
            }
            else if (action == "delete")
            {
                return View("Delete");
            }
            if (action == "MakeVerified")
            {
                foreach(var item in Ids)
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(item);
                    await _userManager.AddToRoleAsync(user, "verified");
                }
               
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string[] UserId)
        {
            foreach (var i in UserId)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(i);
                if (user != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(user);

                }
            }
            return RedirectToAction("Index", "Admin");
        }



        [HttpPost]
        [ActionName("Ban")]
        public async Task<ActionResult> BanConfirmed(string[] UserId)
        {
            foreach (var i in UserId)
            { var user = await _userManager.FindByIdAsync(i);
                if (!user.LockoutEnabled)
                {
                    IdentityResult result = await _userManager.SetLockoutEnabledAsync(user, true);
                }
                else
                {
                    IdentityResult result = await _userManager.SetLockoutEnabledAsync(user, false);
                }
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}