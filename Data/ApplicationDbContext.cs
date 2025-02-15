using Microsoft.EntityFrameworkCore;
using HospitalPatientSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HospitalPatientSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<Doctor>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the relationship between Doctor and Patient
            builder.Entity<Patient>()
                .HasOne(p => p.Doctor)
                .WithMany(d => d.Patients)
                .HasForeignKey(p => p.DoctorId)
                .IsRequired(false); // Allow nulls if needed
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        // Add other DbSets for other entities here
    }
}
