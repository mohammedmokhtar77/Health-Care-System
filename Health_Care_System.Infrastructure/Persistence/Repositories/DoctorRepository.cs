using Health_Care_System.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_System.Infrastructure.Persistence.Repositories;

public class DoctorRepository(HealthCareContext context)
    : GenericRepository<Doctor, int>(context), IDoctorRepository
{}