using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KraudFan2_0.Models;
using KraudFan2_0.Data;
using KraudFan2_0.Models.AppData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HeyRed.MarkdownSharp;
using KraudFan2_0.Models.AppDataViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using KraudFan2_0.Controllers;
using KraudFan2_0.HelpClass.FullTextSearchAdd;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Globalization;
using System.Data.SqlClient;
using LuceneNetSqlDirectory;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Analysis.Standard;

namespace KraudFan2_0.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        IHostingEnvironment _appEnvironment;
        public ProjectController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _signInManager = signInManager;
            _userManager = userManager;
            db = context;
        }
        [HttpGet]
        [Authorize(Roles = "verified")]
        public IActionResult NewProject()
        {
          
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "verified")]
        public async Task<IActionResult> NewProject(ProjectViewModel proj)
        {
            var options = new MarkdownOptions
            {
                AutoHyperlink = true,
                AutoNewLines = true,
                LinkEmails = true,
                QuoteSingleLine = true,
                StrictBoldItalic = true
            };

            KraudFan2_0.HelpClass.ProjectHelp.NewProjectHelpClass taghelp = new HelpClass.ProjectHelp.NewProjectHelpClass(db);
            List<Tag> tags = taghelp.TagsWork(proj.Tags);
            Markdown mark = new Markdown(options);
            string text = mark.Transform(proj.Content);
            var datetoEnter = DateTime.ParseExact(proj.DateEnd, "yyyy-MM-dd", CultureInfo.InvariantCulture); 
            Project project = new Project
            {
                Image=await AddFileAsync(proj.Image),
                Name = proj.Name,
                Description = proj.Description,
                Content = text,
                Status = "In progress",
                DateCreate = DateTime.Now,
                DateEnd = datetoEnter,
                MinDonate = proj.MinDonate,
                MaxDonate = proj.MaxDonate,
                User = db.Users.Find(_userManager.GetUserId(User)),
                Targets = new List<FinancialTarget>(),
                 
                
            };
            foreach (var item in tags)
            {
                project.ProjectTags.Add( new ProjectTags{Project=project, Tag=item } );
            }
            foreach (var item in proj.Targets)
            {
                if (item == proj.Targets[0])
                {
                }
                else { 
                FinancialTarget temp = new FinancialTarget
                {
                    TotalSum = item.TotalSum,
                    Desctiprion = item.Desctiprion,
                    DateEnd = item.DateEnd,
                };
                project.Targets.Add(temp);
            }
            }
            db.Projects.Add(project);
            db.SaveChanges();
            FullSearchAdd.PutElementInSearch(db.Projects.Last());
            var id = db.Projects.Last().Id;
            return RedirectToAction("ProjectShow",new { id=id});
        }
        public async Task<IActionResult> Index(string tag)
        {
            List<Project> data =new List<Project>();
            if (tag != null)
            {

             List<Project> proj = db.Tags.Where(p => p.Name == tag).SelectMany(p => p.ProjectTags.Select(f => f.Project)).ToList();
                foreach(var item in proj)
                {
                  data.AddRange(db.Projects.Where(p => p.Id == item.Id).Take(1).Include(p => p.User).Include("ProjectTags.Tag").ToList());
                    
                }
            }
            else { 
            data = await db.Projects.OrderByDescending(p=>p.DateCreate).Include(p=>p.User).Include("ProjectTags.Tag").ToListAsync();
            }
            return View(data);
        }
        
        public async Task<IActionResult> Sorted(string sort)
        {
            List<Project> proj = null;
            if (sort == "Best")
            { 
            proj = await db.Projects.OrderByDescending(p=>p.TotalRate).Include(p => p.User).Include("ProjectTags.Tag").ToListAsync();
            }
            if (sort == "Newest")
            {
                proj = await db.Projects.OrderByDescending(p => p.DateCreate).Include(p => p.User).Include("ProjectTags.Tag").ToListAsync();
            }
            if (sort == "ComeToEnd")
            {
                 proj = await db.Projects.OrderBy(p => p.DateCreate).Include(p => p.User).Include("ProjectTags.Tag").ToListAsync();
          
            }

       
            return PartialView("_ProjectsShow",proj);
        }

        public async Task<IActionResult> ProjectShow(int id = 2)
        {
            Project proj = db.Projects.Include(p=>p.ProjectTags).First(f=>f.Id==id);
            db.Comments.Where(p => p.ProjectId == proj.Id).Load();
            db.Targets.Where(p => p.ProjectId == proj.Id).Load();
            db.Rates.Where(p => p.ProjectId == proj.Id).Load();
            var Tags = Enumerable.Empty<Tag>().ToList<Tag>();
            foreach (var item in proj.ProjectTags)
            {
                Tags.Add(db.Tags.First(p => p.Id == item.TagId));
            }
            ViewBag.tags = Tags;
            proj.User = await _userManager.FindByIdAsync(proj.UserId);
            HtmlString htmlString = new HtmlString(proj.Content);
            ViewBag.Content = htmlString;
            return View(proj);
        }
        public async Task<IActionResult> CommentAdd(int id,string text)
        {
            Comment comment = new Comment
            {
                User =await db.Users.FindAsync(_userManager.GetUserId(User)),
                Project =await db.Projects.FindAsync(id),
                Text = text,
                DateCreate = DateTime.Now
            };
            await db.Comments.AddAsync(comment);
            db.SaveChanges();


            return RedirectToAction("ProjectShow", new { id = id });
        }
        private async Task<string> AddFileAsync(IFormFile uploadedFile)
        {
            string pathImg="ImageNotUploaded";
            string path = "/images/" + uploadedFile.FileName;
            if (uploadedFile != null)
            {
                // путь к папке Files
               
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                pathImg = path;
                
            }
            CloudinaryDotNet.Account acc = new CloudinaryDotNet.Account("dezcrzbz1",
                "977992111729524",
                "ae-YPnxHWG9Ah6D5qRU_nfQ3-nw");

            Cloudinary cloudinary = new Cloudinary(acc);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(_appEnvironment.WebRootPath + path),
                PublicId = "sample_id",
                Transformation = new Transformation().Crop("limit").Width(400).Height(400),
                EagerTransforms = new List<Transformation>(),
             
                Tags = "special, for_homepage"
            };
            var uploadResult = cloudinary.Upload(uploadParams);

            if (System.IO.File.Exists(_appEnvironment.WebRootPath + path))
            {
                System.IO.File.Delete(_appEnvironment.WebRootPath + path);
            }

            return uploadResult.Uri.AbsoluteUri;
        }

        public async Task<JsonResult> AutoComplete(string term)
      {
            List<string> result= await db.Tags.Where(p => p.Name.Contains(term)).Select(p=>p.Name).ToListAsync();

            
            var json = Json(result);
           
            return json ;
        }
        [Authorize]
        public async Task<IActionResult> Rating(int projectId,string move)
        {
            ApplicationUser currentUser =await _userManager.GetUserAsync(User);
            Rate rate=new Rate();
            Project proj = db.Projects.Find(projectId);
            var ProjectMarks = db.Rates.Where(p => p.ProjectId == projectId).ToList();
            bool markDontExist = true;
            foreach (var item in ProjectMarks)
            {
                if (item.FromUser == currentUser && markDontExist)
                {
                    markDontExist = false;
                    rate = item;
                }
            }
            if (!markDontExist)
            {
                if (move == "RateUp")
                {
                    if (rate.Mark == -1)
                    {
                        db.Rates.Remove(rate);
                        proj.TotalRate += 1;
                        db.Projects.Update(proj);
                        db.SaveChanges();
                    }
                }
                if (move == "RateDown")
                    if (rate.Mark == 1)
                    {
                        db.Rates.Remove(rate);
                        proj.TotalRate -= 1;

                        db.SaveChanges();
                    }
            }
            else { 
            if (move == "RateUp")
            {
                var m = new Rate
                {
                    FromUser = currentUser,
                    Mark = 1,
                    Project = db.Projects.Find(projectId)
                };
                    proj.TotalRate += 1;
                    proj.Rating.Add(m);
                    db.Projects.Update(proj);
                    db.SaveChanges();
                }
            if(move=="RateDown")
            {
                var m = new Rate
                {
                    FromUser = currentUser,
                    Mark = -1,
                    Project = db.Projects.Find(projectId)
                };
                    proj.TotalRate -= 1;
                    proj.Rating.Add(m);
                    db.Projects.Update(proj);
                    db.SaveChanges();
            }
            }
            return RedirectToAction("ProjectShow", new { id = projectId });
        }
        [HttpPost]
        public IActionResult Search(string querry)
        {
            //index bd
            //IndexAllData();
            List<int> Final;
            using (var connection = new SqlConnection(@"Data Source=SQL6003.SmarterASP.NET;Initial Catalog=DB_A296DE_shchepko;User Id=DB_A296DE_shchepko_admin;Password=15879569pg;MultipleActiveResultSets=True"))
            {
                connection.Open();
                var directory = new SqlServerDirectory(connection, new Options() { SchemaName = "[search]" });
                var exists = !IndexReader.IndexExists(directory);

                var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));
                var KeywordsQuery = parser.Parse(querry);
                var searcher = new IndexSearcher(directory);

                var query = new BooleanQuery
                {
                    { KeywordsQuery, Occur.MUST }
                };
                var hits = searcher.Search(query, 20);

                List<int> projectsId = new List<int>();
                foreach (var project in hits.ScoreDocs)
                {
                    var doc = searcher.Doc(project.Doc);
                    var Id = int.Parse(doc.Get("Id"));
                    projectsId.Add(Id);
                }
                Final = projectsId.Distinct().ToList();

            }
            List<Project> FinalProjects = new List<Project>();
            foreach (var id in Final)
            {
                FinalProjects.AddRange(db.Projects.Where(f => f.Id == id).Include(p => p.User).Include("ProjectTags.Tag").ToList());
            }
            return View("~/Views/Project/Index.cshtml", FinalProjects);
        }

    }
}