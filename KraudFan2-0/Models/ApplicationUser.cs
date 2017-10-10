using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using KraudFan2_0.Models.AppData;
namespace KraudFan2_0.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateRegister { get; set; }
        public DateTime DateLastLogin { get; set; }
        public string locale { get; set; }
        public int Rating { get; set; }
        public string status { get; set; }
        public bool Banned { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Passport { get; set; }
        public virtual ICollection<SignProject> SignedProject { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<UserAchievments> UserAchievments { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public ApplicationUser()
        {
            UserAchievments = new List<UserAchievments>();
            Messages = new List<Message>();
            SignedProject = new List<SignProject>();
            Projects = new List<Project>();
        }
    }
}
