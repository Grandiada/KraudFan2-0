using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace KraudFan2_0.Models.AppData
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public ICollection<ProjectTags> ProjectTags { get; set; }
        public string Status { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Image { get; set; }
        public ICollection<FinancialTarget> Targets { get; set; }
        public ICollection<Rate> Rating { get; set; }
        public int TotalRate { get; set; }
        public decimal? MinDonate { get; set; }
        public decimal? MaxDonate { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public Project()
        {
            ProjectTags = new List<AppData.ProjectTags>();
            Comments = new List<Comment>();
            Targets = new List<FinancialTarget>();
            Rating = new List<Rate>();
        }
    }
}
