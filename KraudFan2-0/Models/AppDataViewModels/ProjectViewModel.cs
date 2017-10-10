using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KraudFan2_0.Models.AppData;
using KraudFan2_0.Models.AppDataViewModels;
using Microsoft.AspNetCore.Http;
namespace KraudFan2_0.Models.AppDataViewModels
{
    public class ProjectViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string DateEnd { get; set; }
        public decimal? MinDonate { get; set; }
        public decimal? MaxDonate { get; set; }
        public FinancialTargetViewModel[] Targets {get;set;}
        public IFormFile Image { get; set; }
        public string Tags { get; set; }
    }
}
