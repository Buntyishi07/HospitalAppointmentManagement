// This namespace contains the model classes of the application.
namespace HospitalAppointmentManagement.Models
{
    // This class represents the details of a doctor.
    public class Doctor
    {
        // This property stores the unique ID of the doctor.
        public int DoctorId { get; set; }

        // This property stores the name of the doctor.
        public string DoctorName { get; set; } = "";

        // This property stores the specialization of the doctor.
        public string Specialization { get; set; } = "";

        // This property stores the years of experience of the doctor.
        public int Experience { get; set; }

        // This property stores the consultation fee of the doctor.
        public decimal ConsultationFee { get; set; }
    }
}