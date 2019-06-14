using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluation.Models
{
    public class Course
    {
        public int cid{ get; set; }
        public string title { get; set; }
        public string img { get; set; }
        public string code { get; set; }

        public string isActive { get; set; }
        public int levelId { get; set; }
    //    public string LevVal { get; set; }


    }
}