public class Doctor
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    // One Doctor has many Appointments
    public ICollection<Appointment> Appointments { get; set; }
        = new List<Appointment>();
}