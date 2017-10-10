using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.Models.AppData
{
    public class UserAchievments
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Achievment> achievments { get; set; }

    }
}
