using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.Models.AppData
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectTags> ProjectTags { get; set; }
        public Tag()
        {
            ProjectTags = new List<AppData.ProjectTags>();
        }
    }
}
