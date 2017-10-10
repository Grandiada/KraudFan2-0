using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.Models.AppData
{
    public class FinancialTarget
    {
        public int Id { get; set; }
        public decimal TotalSum { get; set; }
        public decimal CurentSum { get; set; }
        public string Desctiprion { get; set; }
        public bool Status { get; set; }
        public DateTime DateEnd { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }

    }
}
