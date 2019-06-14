using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluation.Models
{
    public class Question
    {
      
        public int quesId { get; set; }
        public string ques { get; set; }
        public string isActive { get; set; }
        public int secID { get; set; }
        public int rateId { get; set; }
        public int counter { get; set; }

    }
}