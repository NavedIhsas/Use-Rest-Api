using System.ComponentModel.DataAnnotations;

namespace Catologs.Models
{
	public class LoginDto
	{
		[Required(ErrorMessage ="لطفا رمز عبور را وارد کنید")]
        public string Password { get; set; }
    }
}
