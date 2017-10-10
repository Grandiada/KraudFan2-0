using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lucene.Net;
using LuceneNetSqlDirectory;
using KraudFan2_0.Data;
using KraudFan2_0.Models.AppData;
using KraudFan2_0.Models.FullTextSearchModels;
using System.Data.SqlClient;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Microsoft.EntityFrameworkCore;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
namespace KraudFan2_0.Controllers
{
    //"Server=DESKTOP-S0G3F8R\\SQLEXPRESS;Database=KraudFan2_0;Trusted_Connection=True;MultipleActiveResultSets=true"
    public class FullTextSearchController : Controller
    {
        ApplicationDbContext db;
        public FullTextSearchController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public List<FullTextSearchModelcs> TransformAll()
        {
            List<FullTextSearchModelcs> transformed = new List<FullTextSearchModelcs>();
            var projects = db.Projects.ToList();
            //project transform
            foreach (var item in projects)
            {
                List<FullTextSearchModelcs> temp = new List<FullTextSearchModelcs>
                {
                    new FullTextSearchModelcs{ProjectId=item.Id,Content=item.Name },
                    new FullTextSearchModelcs{ProjectId=item.Id,Content=item.Description },
                    new FullTextSearchModelcs{ProjectId=item.Id,Content=item.Content }
                };
                transformed.AddRange(temp);
            }
            //comment transform
            var comments = db.Comments.Include(p=>p.User).ToList();
            foreach (var item in comments)
            {
                List<FullTextSearchModelcs> temp = new List<FullTextSearchModelcs>
                {
                    new FullTextSearchModelcs{ProjectId=item.ProjectId,Content=item.User.UserName },
                    new FullTextSearchModelcs{ProjectId=item.ProjectId,Content=item.Text }
                };
                transformed.AddRange(temp);
            }
            return transformed;
        }

        public IActionResult IndexAllData()
        {
            using (var connection = new SqlConnection(@"Data Source=SQL6003.SmarterASP.NET;Initial Catalog=DB_A296DE_shchepko;User Id=DB_A296DE_shchepko_admin;Password=15879569pg;MultipleActiveResultSets=True"))
            {
                connection.Open();
                SqlServerDirectory.ProvisionDatabase(connection, schemaName: "[search]", dropExisting: true);
                var directory = new SqlServerDirectory(connection, new Options() { SchemaName = "[search]" });

                var exists = !IndexReader.IndexExists(directory);
                var indexWriter = new IndexWriter(directory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), exists, new Lucene.Net.Index.IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH));
                var coll = TransformAll();

                foreach(var item in coll)
                {
                    var doc = new Document();
                    doc.Add(new NumericField("Id", Field.Store.YES, true).SetIntValue(item.ProjectId));
                    if (item.Content == null)
                    {
                        doc.Add(new Field("Title", "Govnina", Field.Store.NO, Field.Index.ANALYZED, Field.TermVector.NO));
                    }
                    else
                    {
                        doc.Add(new Field("Title", item.Content, Field.Store.NO, Field.Index.ANALYZED, Field.TermVector.NO));
                    }
                    indexWriter.AddDocument(doc);
                }
               // indexWriter.Optimize();
                indexWriter.Flush(true, true, true);
                indexWriter.Commit();
                indexWriter.Dispose();
                

            }
            return RedirectToAction("Index", "Home");
        }

       
    }
}