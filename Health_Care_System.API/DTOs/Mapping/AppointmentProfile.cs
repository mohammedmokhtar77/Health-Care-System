using AutoMapper;
using Health_Care_System.API.DTOs.Appointments;

namespace Health_Care_System.API.DTOs.Mapping;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentDto>()
            .ForMember
            (
                dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.Name)
            )
            .ForMember(
                dest => dest.PatientName,
                opt => opt.MapFrom(
                    src => src.Patient.Name)
            );

        CreateMap<CreateAppointmentDto, Appointment>();
        CreateMap<UpdateAppointmentDto, Appointment>();
    }
}