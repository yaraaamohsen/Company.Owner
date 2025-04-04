using System.ComponentModel.DataAnnotations;

namespace Company.Owner.PL.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = "User Name is required!!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "FirstName is required!!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required!!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required!!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required!!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+20(10|11|12|15)\d{8}$", ErrorMessage = "Phone Number must be +201xxxxxxxxx")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required!!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Password Must Contain at least 1 Big Letter, 1 Small Letter, Charchter, Number, And More Than 8 Charchters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required!!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and ConfirmPassword does not match!!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Must agree on terms and conditions")]
        public bool IsAgree { get; set; }
    }
}
