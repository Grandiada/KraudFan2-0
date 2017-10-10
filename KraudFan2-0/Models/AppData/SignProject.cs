using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.Models.AppData
{
    public class SignProject
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
