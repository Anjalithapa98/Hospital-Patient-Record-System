namespace HospitalPatientSystem.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public string RecordDetails { get; set; } = string.Empty;

        // Foreign key
        public int PatientId { get; set; }

        // Navigation property
        public Patient? Patient { get; set; }
    }
}
