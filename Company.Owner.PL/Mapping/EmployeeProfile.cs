using AutoMapper;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;

namespace Company.Owner.PL.Mapping
{
    public class EmployeeProfile : Profile  
    {
        public EmployeeProfile()
        {
            //CreateMap<CreateEmployeeDto, Employee>(); // Search With The Same Name And Datatype
            CreateMap<CreateEmployeeDto, Employee>().ReverseMap();

            //CreateMap<CreateEmployeeDto, Employee>()
            //    .ForMember(D => D.Name, O => O.MapFrom(S=> S.Name)); // To specify Different Names   


        }
    }
}
