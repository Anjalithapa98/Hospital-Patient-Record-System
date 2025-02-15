using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HospitalPatientSystem.Models
{
    public class Doctor : IdentityUser
    {
        // Ensure Id is unique if overriding it, otherwise it's handled by IdentityUser.
        public new int Id { get; set; }

        // Strings can be nullable or assigned empty, depending on your needs.
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;

        // Custom fields for Email and Password, if necessary.
        public new string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Specialty { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;

        // Navigation properties with nullable types.
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<Treatment>? Treatments { get; set; }

        // Patients associated with this Doctor.
        public ICollection<Patient>? Patients { get; set; }
    }
}
