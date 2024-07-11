namespace new_doctors.models
{
    public class VerifyEmail
    {
        public int Id { get; set; }

        public string Token { get; set; }
        public string Email { get; set; }
    }
}
