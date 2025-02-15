using System;

namespace HospitalPatientSystem.Models
{
    public class Treatment
    {
        public int Id { get; set; }
        public string TreatmentDescription { get; set; } = string.Empty;
        public DateTime TreatmentDate { get; set; }

        // Foreign keys
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        // Navigation properties
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
