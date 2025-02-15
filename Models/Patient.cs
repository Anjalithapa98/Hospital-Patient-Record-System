using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalPatientSystem.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        // Calculate age dynamically instead of storing it
        public int Age => DateTime.Now.Year - DateOfBirth.Year;

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Number is required.")]
        [StringLength(15, ErrorMessage = "Contact Number cannot be longer than 15 characters.")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doctor is required.")]
        public int DoctorId { get; set; }  // Foreign key to the Doctor model
        public Doctor? Doctor { get; set; } // Navigation property to the Doctor

        // Navigation properties
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<Treatment>? Treatments { get; set; }
        public MedicalRecord? MedicalRecord { get; set; }
    }
}
