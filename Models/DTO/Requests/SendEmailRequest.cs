using System.ComponentModel.DataAnnotations;

namespace dotnet5_webapp.Models.DTO.Requests
{
    public class SendEmailRequest
    {
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}