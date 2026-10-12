namespace Health_Care_System.Entities;
public class Patient
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    // One Patient has many Appointments
    public ICollection<Appointment> Appointments { get; set; }
        = new List<Appointment>();
}