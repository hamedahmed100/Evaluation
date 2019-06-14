using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evaluation.Models
{
    public class UserInfo
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string  isActive { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Make sure to ENTER The Password")]
        public string pwd { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Make sure to ENTER The Confirm Password")]
        [Compare("Pwd")]
        public string confirmPwd { get; set; }

        
     

     
        public int authId { get; set; }

        public int Count { get; set; }

        public int maxId { get; set; }
    }
}