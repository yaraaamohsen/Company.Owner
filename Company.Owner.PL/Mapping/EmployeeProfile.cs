using AutoMapper;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;

namespace Company.Owner.PL.Mapping
{
    public class EmployeeProfile : Profile  
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>().ReverseMap();
        }
    }
}
