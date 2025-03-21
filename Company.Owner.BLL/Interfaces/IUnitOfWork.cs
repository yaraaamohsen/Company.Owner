using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Owner.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IDepartmentRepository departmentRepository { get; }
        public IEmployeeRemository employeeRemository { get; }
    }
}
