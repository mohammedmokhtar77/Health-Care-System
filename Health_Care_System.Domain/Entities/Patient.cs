using Health_Care_System.Domain.Entities.Common;

namespace Health_Care_System.Domain.Entities;
public class Patient : AuditableEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    // One Patient has many Appointments
    public ICollection<Appointment> Appointments { get; set; }
        = new List<Appointment>();
}