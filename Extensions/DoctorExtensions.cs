// This namespace contains extension methods used by the application.
namespace HospitalAppointmentManagement.Extensions
{
    // This class contains extension methods for the Doctor model.
    public static class DoctorExtensions
    {
        // This method calculates the total consultation charge.
        // It adds a service charge of ₹100 to the doctor's consultation fee.
        public static decimal CalculateTotalCharge(this Models.Doctor doctor)
        {
            // Add ₹100 service charge to the consultation fee.
            return doctor.ConsultationFee + 100;
        }
    }
}