using KraudFan2_0.Data;
using KraudFan2_0.Models;
using KraudFan2_0.Models.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.HelpClass.ProjectHelp
{
    public class NewProjectHelpClass
    {
        private ApplicationDbContext db;
  
        public NewProjectHelpClass(ApplicationDbContext context)
        {
            db = context;
        }

        public List<Tag> TagsWork(string tags)
        {
            string[] separated=tags.Split(new Char[] { ',', ' ' });

           var Tags= Enumerable.Empty<Tag>().ToList<Tag>();
           
            
            foreach (var item in separated)
            {
                if (item != "")
                {
                    if (db.Tags.Any(p=>p.Name==item))
                    {
                        Tags.Add(db.Tags.First(p => p.Name == item));
                    }
                    else
                    {
                        db.Tags.Add(new Tag { Name = item });
                        db.SaveChanges();
                        Tags.Add(db.Tags.Last());
                    }
                }
            }

            return Tags;
        }



    }
}
