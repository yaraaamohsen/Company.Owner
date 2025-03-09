using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.Owner.PL.Dtos
{
    public class DetailsDepartmentDto
    {
        [ReadOnly(true)]
        public int Code { get; set; }

        [ReadOnly(true)]
        public string Name { get; set; }

        [ReadOnly(true)]
        public DateTime CreateAt { get; set; }
    }
}
