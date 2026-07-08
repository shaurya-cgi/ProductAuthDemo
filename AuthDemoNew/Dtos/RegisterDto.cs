using System.ComponentModel.DataAnnotations;

namespace AuthDemoNew.Dtos
{
    public class RegisterDto
    {

        public required string Username { get; set; }

        public required string PasswordHash { get; set; }


    }
}

