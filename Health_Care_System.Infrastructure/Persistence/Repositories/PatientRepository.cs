using Health_Care_System.Domain.Entities;
using Health_Care_System.Domain.Repositories.Interfaces;

namespace Health_Care_System.Infrastructure.Persistence.Repositories;

public class PatientRepository(HealthCareContext context)
    : GenericRepository<Patient, int>(context), IPatientRepository {}