namespace new_doctors.DTOs
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string ImageUrl { get; set; }
        public string LastName { get; set; }
        public string? CvUrl { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
