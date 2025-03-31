using System.ComponentModel.DataAnnotations;

namespace Company.Owner.PL.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "New Password is required!!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "New Password Must Contain at least 1 Big Letter, 1 Small Letter, Charchter, Number, And More Than 8 Charchters")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password is required!!")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Password and Confirm New Password does not match!!")]
        public string ConfirmNewPassword { get; set; }

    }
}
