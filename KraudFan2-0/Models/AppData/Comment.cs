using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.Models.AppData
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime DateCreate { get; set; }
        public string Text { get; set; }
        public ICollection<Rate> Rating { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
