using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.Models.AppData
{
    public class Message
    {
        public int Id { get; set; }
        public string Theme { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
