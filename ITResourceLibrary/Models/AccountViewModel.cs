using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ITResourceLibrary.Models
{
    public class AccountViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不填就没法提交")]
        public string User { get; set; }
        [Required]
        public string Password { get; set; }
    }
}