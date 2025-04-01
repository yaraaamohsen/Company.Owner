using System.ComponentModel.DataAnnotations;

namespace Company.Owner.PL.Dtos
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Email is required!!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Password Must Contain at least 1 Big Letter, 1 Small Letter, Charchter, Number, And More Than 8 Charchters")]
        public string Password { get; set; }
        public bool RemeberMe { get; set; }
    }
}
