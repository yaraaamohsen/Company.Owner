using AutoMapper;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;

namespace Company.Owner.PL.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDto, Department>().ReverseMap();
        }
    }
}
