// This namespace contains the model classes of the application.
namespace HospitalAppointmentManagement.Models
{
    // This class represents a post received from JSONPlaceholder.
    public class Post
    {
        // This property stores the user ID of the post.
        public int UserId { get; set; }

        // This property stores the ID of the post.
        public int Id { get; set; }

        // This property stores the title of the post.
        public string Title { get; set; } = "";

        // This property stores the body of the post.
        public string Body { get; set; } = "";
    }
}