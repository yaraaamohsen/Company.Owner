using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Company.Owner.DAL.Models;

namespace Company.Owner.PL.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Name Is Required !")]
        public string Name { get; set; }

        [Range(22,60, ErrorMessage = "Age Is Out Of Range, It Must Be Between 22 And 60")]
        public int? Age { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [RegularExpression(@"^[1-9]{1,3}-[A-Za-z]{2,10}-[A-Za-z]{2,10}-[A-Za-z]{2,10}$", ErrorMessage ="Address must be 123-street-City-Region")]
        public string Address { get; set; }

        [RegularExpression(@"^\+20(10|11|12|15)\d{8}$", ErrorMessage = "Phone Number must be +201xxxxxxxxx")]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Email Is Not Valid")]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }

        [DisplayName("Date Of Creation")]
        public DateTime CreateAt { get; set; }
        [DisplayName("Department")]
        public int? DepartmentId { get; set; }

        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
