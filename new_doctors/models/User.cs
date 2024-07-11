namespace new_doctors.models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrivateNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ResetCode { get; set; }   
    }
}
