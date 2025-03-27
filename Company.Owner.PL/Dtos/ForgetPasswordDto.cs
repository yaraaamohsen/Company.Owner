using System.ComponentModel.DataAnnotations;

namespace Company.Owner.PL.Dtos
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email is not valid")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
