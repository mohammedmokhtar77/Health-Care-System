using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Entities.Common;

public class Appointment : AuditableEntity
{
    // Composite Primary Key + Foreign Key
    public int PatientId { get; set; }

    // Composite Primary Key + Foreign Key
    public int DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    // Navigation Properties
    public Patient Patient { get; set; } = null!;

    public Doctor Doctor { get; set; } = null!;
}