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
    public class TagCanvas : ViewComponent
    {
        ApplicationDbContext db;
        public TagCanvas(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IViewComponentResult Invoke(int id)
        {
           
            List<Tag> tags = db.Tags.OrderByDescending(p=>p.ProjectTags.Count()).Take(20).ToList();
            return View(tags);
        }
    }
}
