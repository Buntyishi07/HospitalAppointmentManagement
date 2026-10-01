// This namespace contains the controllers of the application.
namespace HospitalAppointmentManagement.Controllers
{
    // Import the extension method namespace.
    using HospitalAppointmentManagement.Extensions;

    // Import the model namespace.
    using HospitalAppointmentManagement.Models;

    // Import ASP.NET Core MVC classes.
    using Microsoft.AspNetCore.Mvc;

    // Import memory caching classes.
    using Microsoft.Extensions.Caching.Memory;

    // Import HTTP JSON functionality.
    using System.Net.Http.Json;

    // This controller handles the main pages of the hospital application.
    public class HomeController : Controller
    {
        // This object is used to store and retrieve data from memory cache.
        private readonly IMemoryCache _memoryCache;

        // This object is used to make HTTP requests asynchronously.
        private readonly HttpClient _httpClient;

        // This constructor receives the required services from dependency injection.
        public HomeController(IMemoryCache memoryCache, HttpClient httpClient)
        {
            // Store the memory cache service in the private variable.
            _memoryCache = memoryCache;

            // Store the HTTP client in the private variable.
            _httpClient = httpClient;
        }

        // This action displays the home page and doctor list.
        public IActionResult Index()
        {
            // Create a key that will be used to identify the doctor list in cache.
            const string cacheKey = "DoctorList";

            // Try to get the doctor list from memory cache.
            if (!_memoryCache.TryGetValue(cacheKey, out List<Doctor>? doctors))
            {
                // Create a list of doctors when the data is not already cached.
                doctors = GetDoctors();

                // Set the doctor list in memory cache.
                // The cached data will expire after 3 minutes.
                _memoryCache.Set(
                    cacheKey,
                    doctors,
                    TimeSpan.FromMinutes(3)
                );
            }

            // Return the doctor list to the Index view.
            return View(doctors);
        }

        // This method creates sample doctor information.
        private List<Doctor> GetDoctors()
        {
            // Return a list containing doctor information.
            return new List<Doctor>
            {
                // Create the first doctor object.
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Dr. Rajesh Patel",
                    Specialization = "Cardiologist",
                    Experience = 12,
                    ConsultationFee = 700
                },

                // Create the second doctor object.
                new Doctor
                {
                    DoctorId = 2,
                    DoctorName = "Dr. Priya Shah",
                    Specialization = "Dermatologist",
                    Experience = 8,
                    ConsultationFee = 500
                },

                // Create the third doctor object.
                new Doctor
                {
                    DoctorId = 3,
                    DoctorName = "Dr. Amit Mehta",
                    Specialization = "Neurologist",
                    Experience = 15,
                    ConsultationFee = 900
                },

                // Create the fourth doctor object.
                new Doctor
                {
                    DoctorId = 4,
                    DoctorName = "Dr. Neha Desai",
                    Specialization = "Pediatrician",
                    Experience = 10,
                    ConsultationFee = 600
                },

                // Create the fifth doctor object.
                new Doctor
                {
                    DoctorId = 5,
                    DoctorName = "Dr. Kunal Joshi",
                    Specialization = "Orthopedic",
                    Experience = 9,
                    ConsultationFee = 650
                }
            };
        }

        // This action displays details of a particular doctor.
        // ResponseCache enables response caching for this page.
        [ResponseCache(Duration = 60)]
        public IActionResult Details(int DoctorId)
        {
            // Get the doctor list from the memory cache.
            if (!_memoryCache.TryGetValue("DoctorList", out List<Doctor>? doctors))
            {
                // Create the doctor list if it is not currently cached.
                doctors = GetDoctors();

                // Store the doctor list in memory cache for 3 minutes.
                _memoryCache.Set(
                    "DoctorList",
                    doctors,
                    TimeSpan.FromMinutes(3)
                );
            }

            // Find the doctor whose ID matches the query string value.
            Doctor? doctor = doctors.FirstOrDefault(d => d.DoctorId == DoctorId);

            // If no doctor is found, display a not-found result.
            if (doctor == null)
            {
                return NotFound();
            }

            // Calculate the total consultation charge using the extension method.
            decimal totalCharge = doctor.CalculateTotalCharge();

            // Store the calculated charge in ViewBag.
            ViewBag.TotalCharge = totalCharge;

            // Return the doctor details to the Details view.
            return View(doctor);
        }

        // This action displays the appointment form.
        public IActionResult Appointment(int DoctorId)
        {
            // Get the doctor list from memory cache.
            if (!_memoryCache.TryGetValue("DoctorList", out List<Doctor>? doctors))
            {
                // Create the doctor list if it is not cached.
                doctors = GetDoctors();

                // Store the list in memory cache for 3 minutes.
                _memoryCache.Set(
                    "DoctorList",
                    doctors,
                    TimeSpan.FromMinutes(3)
                );
            }

            // Find the selected doctor using the DoctorId.
            Doctor? doctor = doctors.FirstOrDefault(d => d.DoctorId == DoctorId);

            // Return an error if the doctor does not exist.
            if (doctor == null)
            {
                return NotFound();
            }

            // Store the selected doctor in ViewBag for displaying on the form.
            ViewBag.Doctor = doctor;

            // Return the appointment view.
            return View();
        }

        // This action receives the submitted appointment form.
        [HttpPost]
        public IActionResult Appointment(
            int DoctorId,
            string PatientName,
            string Specialization,
            DateTime AppointmentDate)
        {
            // Store the patient name in a cookie.
            Response.Cookies.Append("PatientName", PatientName);

            // Store the selected specialization in the session.
            HttpContext.Session.SetString("Specialization", Specialization);

            // Store the patient name in ViewBag for the confirmation message.
            ViewBag.PatientName = PatientName;

            // Store the doctor ID in ViewBag.
            ViewBag.DoctorId = DoctorId;

            // Store the specialization in ViewBag.
            ViewBag.Specialization = Specialization;

            // Store the selected appointment date in ViewBag.
            ViewBag.AppointmentDate = AppointmentDate;

            // Return the appointment confirmation view.
            return View("Appointment");
        }

        // This asynchronous action retrieves posts from JSONPlaceholder.
        public async Task<IActionResult> Posts()
        {
            // Request posts asynchronously from the JSONPlaceholder API.
            List<Post>? posts = await _httpClient.GetFromJsonAsync<List<Post>>(
                "https://jsonplaceholder.typicode.com/posts"
            );

            // If the API does not return data, create an empty list.
            posts ??= new List<Post>();

            // Return the posts to the Posts view.
            return View(posts);
        }

        // This action displays stored cookie and session information.
        public IActionResult PatientInfo()
        {
            // Read the patient name from the cookie.
            string patientName = Request.Cookies["PatientName"] ?? "Not available";

            // Read the specialization from the session.
            string specialization =
                HttpContext.Session.GetString("Specialization") ?? "Not available";

            // Store the cookie value in ViewBag.
            ViewBag.PatientName = patientName;

            // Store the session value in ViewBag.
            ViewBag.Specialization = specialization;

            // Display the PatientInfo view.
            return View();
        }
    }
}