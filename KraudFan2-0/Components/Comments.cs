using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KraudFan2_0.Data;
using Microsoft.EntityFrameworkCore;
using KraudFan2_0.Models.AppData;
namespace KraudFan2_0.Components
{
    public class Comments : ViewComponent
    {
        ApplicationDbContext db;
        public Comments(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke(int id)
        {
            var items = db.Comments.Where(p => p.ProjectId == id).Include(f=>f.User).ToList();
            return View(items);
        }
    }
}
