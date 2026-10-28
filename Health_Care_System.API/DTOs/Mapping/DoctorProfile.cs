using AutoMapper;
using Health_Care_System.API.DTOs.Doctors;

namespace Health_Care_System.API.DTOs.Mapping;

public class DoctorProfile : Profile
{
    public DoctorProfile()
    {
        CreateMap<Doctor, DoctorDto>();
        CreateMap<CreateDoctorDto, Doctor>();
        CreateMap<UpdateDoctorDto, Doctor>();
    }
}