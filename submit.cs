using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluation.Models
{
    public class submit
    {
        public int userId { get; set; }
        public int cid { get; set; }
        public int semId { get; set; }
        public int quesId { get; set; }
        public int rateId { get; set; }
        public int ratevalue { get; set; }


        public string title { get; set; }
        public string date { get; set; }
        public string time { get; set; }


    }
}