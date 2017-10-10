using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
namespace KraudFan2_0.HelpClass.FullTextSearchAdd
{
    public static class FullSearchAdd
    {
        public static void PutElementInSearch(Project proj)
        {
            List<FullTextSearchModelcs> model = new List<FullTextSearchModelcs> {
                new FullTextSearchModelcs { ProjectId = proj.Id, Content=proj.Name },
                new FullTextSearchModelcs { ProjectId = proj.Id, Content=proj.Description },
                new FullTextSearchModelcs { ProjectId = proj.Id, Content=proj.Content },
            };
            AddSearchIndex(model);
        }
        public static void PutElementInSearch(Comment comment)
        {
            List<FullTextSearchModelcs> model = new List<FullTextSearchModelcs> { new FullTextSearchModelcs { ProjectId = comment.ProjectId, Content = comment.Text } };
            AddSearchIndex(model);
        }

        public static void AddSearchIndex(List<FullTextSearchModelcs> Adding)
        {
            using (var connection = new SqlConnection(@"Server=DESKTOP-S0G3F8R\SQLEXPRESS;Database=KraudFan2_0;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                connection.Open();
                var directory = new SqlServerDirectory(connection, new Options() { SchemaName = "[search]" });
                
                var exists = IndexReader.IndexExists(directory);
                if (exists == false)
                {
                    SqlServerDirectory.ProvisionDatabase(connection, schemaName: "[search]", dropExisting: true);
                }
                var indexWriter = new IndexWriter(directory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), exists, new Lucene.Net.Index.IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH));

                foreach (var item in Adding)
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
                
                indexWriter.Flush(true,true,true);

                indexWriter.Dispose();

            }
        }

    }
}
