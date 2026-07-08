namespace AuthDemoNew.Dtos
{
    public class RegisterDto
    {

        public required string Username { get; set; }

        public required string Password { get; set; }

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

    }
}
