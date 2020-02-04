using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortal.Models
{
    public class RankingReport
    {
        public string DepartmentName { get; set; }
        public int Placement { get; set; }
        public double AvgPoint { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }

    }
}
