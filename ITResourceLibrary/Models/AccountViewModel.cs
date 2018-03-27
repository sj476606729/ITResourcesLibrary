using System.ComponentModel.DataAnnotations;

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