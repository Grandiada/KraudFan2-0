using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KraudFan2_0.Data;
using Microsoft.EntityFrameworkCore;
using KraudFan2_0.Models.AppData;
using KraudFan2_0.Models.AppDataViewModels;
using KraudFan2_0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace KraudFan2_0.Components
{
    public class Rating : ViewComponent
    {
        IHostingEnvironment _appEnvironment;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public Rating(ApplicationDbContext _db, 
            UserManager<ApplicationUser> userManager, 
            IHostingEnvironment appEnvironment,
            SignInManager<ApplicationUser> signInManager)
        {
            _appEnvironment =appEnvironment;
            db = _db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IViewComponentResult Invoke(int id,string currentUser)
        {
            RatingViewModel rait = new RatingViewModel();
            var ProjectMarks = db.Rates.Where(p => p.ProjectId == id).ToList();
            rait.ProjectId = id;
            rait.AvailableUp = true;
            rait.AbailableDown = true;

            if (currentUser != null)
            {
                ApplicationUser user = db.Users.Find(currentUser);
                int i = 0;
                bool markDontExist=true;
                foreach (var item in ProjectMarks)
                {
                    if (item.FromUser == user&&markDontExist)
                    {
                        markDontExist = false;
                        if(item.Mark == 1)
                        {
                            rait.AvailableUp = false;
                        }
                        if (item.Mark == -1)
                        {
                            rait.AbailableDown = false;
                        }

                    }
                    i += item.Mark;
                }
                rait.TotalRating = i;

            }
            else
            {
                int i = 0;
                foreach (var item in ProjectMarks)
                {
                    i += item.Mark;
                }
                rait.TotalRating = i;
            }
            return View(rait);
        }
    }
}
