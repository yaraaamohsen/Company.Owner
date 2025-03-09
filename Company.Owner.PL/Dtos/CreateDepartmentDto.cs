using System.ComponentModel.DataAnnotations;

namespace Company.Owner.PL.Dtos
{
    public class CreateDepartmentDto
    {
        [Required (ErrorMessage ="Code Is Required !")]
        public int Code { get; set; }

        [Required(ErrorMessage = "Name Is Required !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date Is Required !")]
        public DateTime CreateAt { get; set; }
    }
}
