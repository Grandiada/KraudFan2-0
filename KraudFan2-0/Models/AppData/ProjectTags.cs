﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KraudFan2_0.Models.AppData
{
    public class ProjectTags
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
