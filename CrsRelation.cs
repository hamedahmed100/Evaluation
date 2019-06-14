using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluation.Models
{
    public class CrsRelation
    {
        public int cid { get; set; }
        
        public int semId { get; set; }

        public int year { get; set; }
        public int docId { get; set; }
        public int assistantId { get; set; }
        public int speId { get; set; }
        //public string docValue { get; set; }

        //public string assistantValue { get; set; }
        //public string semValue { get; set; }

    }
}