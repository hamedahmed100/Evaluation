using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluation.Models
{
    public class RateQuesTable
    {
        public int quesId { get; set; }
        public int rateId { get; set; }

        public int weak { get; set; }

        public int normal { get; set; }

        public int good { get; set; }
        public int verygood { get; set; }

        public int distinct { get; set; }

    }
}