using System.ComponentModel.DataAnnotations;

namespace dotnet5_webapp.Models.DTO.Requests
{
    public class ResetPasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}