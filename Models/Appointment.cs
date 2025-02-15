using System;

namespace HospitalPatientSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }

        // Foreign keys
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        // Navigation properties
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
