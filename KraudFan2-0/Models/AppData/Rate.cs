using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.Models.AppData
{
    public class Rate
    {
        public int Id { get; set; }
        public ApplicationUser FromUser { get; set; }
        public int Mark { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
