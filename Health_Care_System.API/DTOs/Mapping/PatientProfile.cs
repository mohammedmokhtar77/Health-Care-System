using AutoMapper;
using Health_Care_System.API.DTOs.Doctors;
using Health_Care_System.API.DTOs.Patients;
using Health_Care_System.Domain.Entities;

namespace Health_Care_System.API.DTOs.Mapping;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<Patient, PatientDto>()
            .ForCtorParam(
                nameof(PatientDto.FullName),
                options => options.MapFrom(source => source.Name));        CreateMap<CreatePatientDto, Patient>();
        CreateMap<UpdatePatientDto, Patient>();
    }
}