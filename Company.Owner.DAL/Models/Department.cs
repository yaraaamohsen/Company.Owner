using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Owner.DAL.Models
{
    internal class Department
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int Name { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
