using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.Models.AppDataViewModels
{
    public class RatingViewModel
    {
        public int ProjectId { get; set; }
        public int TotalRating { get; set; }
        public bool AvailableUp { get; set; }
        public bool AbailableDown { get; set; }

    }
}
