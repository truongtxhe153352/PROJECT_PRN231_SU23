using AutoMapper;

namespace CourseAPI.MappingModel
{
    public class MappingEntity : Profile
    {
        public MappingEntity()
        {
            //CreateMap<Employee, EmployeeDTO>()
            //    .ForMember(dest => dest.BirthDateStr, dest => dest.MapFrom(src => (src.BirthDate.Value.ToShortDateString())))
            //    .ForMember(dest => dest.LastName, dest => dest.MapFrom(src => src.LastName))
            //    .ForMember(dest => dest.FirstName, dest => dest.MapFrom(src => src.FirstName))
            //    .ForMember(dest => dest.FullName, dest => dest.MapFrom(src => (src.FirstName + " " + src.LastName)))
            //    .ForMember(dest => dest.ReportsTo, dest => dest.MapFrom(src => src.ReportsToNavigation == null ? "No one" : (src.ReportsToNavigation.FirstName + " " + src.ReportsToNavigation.LastName)));
        
        
        }
    }
}
