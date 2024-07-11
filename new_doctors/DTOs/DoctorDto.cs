using System.ComponentModel.DataAnnotations;

namespace new_doctors.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

   

        [Required]
        public string Category { get; set; }

        public string PrivateNumber { get; set; }

        public string ImageUrl { get; set; }

        public string CVUrl { get; set; }

        public string Achievements { get; set; }
    }
}
