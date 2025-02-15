using Microsoft.EntityFrameworkCore;
using HospitalPatientSystem.Models;
using System.Collections.Generic;

namespace HospitalPatientSystem.Data
{
    public class HospitalPatientSystemDbContext : DbContext
    {
        public HospitalPatientSystemDbContext(DbContextOptions<HospitalPatientSystemDbContext> options)
        : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
    }
}
