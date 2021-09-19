using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dotnet5_webapp.Configuration;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Models;
using dotnet5_webapp.Models.DTO.Requests;
using dotnet5_webapp.Models.DTO.Responses;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

namespace dotnet5_webapp.Controllers
{
    [Route("api/[controller]")] // api/authManagement
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPasswordValidator<IdentityUser> _passwordValidator;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor, IConfiguration configuration, IPasswordValidator<IdentityUser> passwordValidator)
        {
            _userManager = userManager;
            _passwordValidator = passwordValidator;
            _jwtConfig = optionsMonitor.CurrentValue;
            Configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return Ok(new RegistrationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Email already in use"
                        },
                        Success = false
                    });
                }

                var newUser = new ApplicationUser() {Email = user.Email, UserName = user.UserName};
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);
                    return Ok(new RegistrationResponse()
                    {
                        Success = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return Ok(new RegistrationResponse()
                    {
                        Errors = isCreated.Errors.Select(e => e.Description).ToList(),
                        Success = false
                    });
                }
            }

            return Ok(new RegistrationResponse()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Success = false
            });
        }
        
        [HttpGet("check")]
        [Authorize]
        public async Task<ActionResult<ResponseWrapper<string>>> TestAuth()
        {
            var response = new ResponseWrapper<string>
            {
                Success = true,
                Status = "",
                Data = ""
            };
            
            var username = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

            if (username == null)
            {
                response.Success = false;
                response.Status = "Unable to automatically authenticate the current user.";
                return Unauthorized(response);
            }

            response.Data = username;
            return Ok(response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid login request"
                        },
                        Success = false
                    });
                }

                var isValid = await _userManager.CheckPasswordAsync(existingUser, user.Password);
                if (!isValid)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid login request"
                        },
                        Success = false
                    });
                    
                }

                var jwtToken = GenerateJwtToken(existingUser);
                return Ok(new RegistrationResponse()
                {
                    Success = true,
                    Token = jwtToken
                });
            }
            
            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Success = false
            });
        }
        
        [HttpPost]
        [Route("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return Ok(new RegistrationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid user"
                        },
                        Success = false
                    });
                }
                
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var wepAppUrl = Configuration.GetSection("ReactAppUrl").Value;

                var url = $"{wepAppUrl}/account/passwordreset?token={token}&email={user.Email}";
                var htmlBody =
                    $"<h1>Hey, it happens, no worries</h1><br /><a href=\"{url}\">Click here to reset your password!</a>";
                // var callback = Url.Action(nameof(ResetPassword), new { token, email = user.Email });
                await SendEmail(user.Email, "Password Reset", htmlBody);
                return Ok();
            }
            
            return Ok(new RegistrationResponse()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Success = false
            });
        }
        
        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>()
                    {
                        "Invalid user"
                    },
                    Success = false
                });
            }
            
            var result = await _passwordValidator.ValidateAsync(_userManager, user, resetPasswordModel.Password);
            if (!result.Succeeded)
            {
                return Ok(new RegistrationResponse()
                {
                    Errors = result.Errors.Select(e => e.Description).ToList(),
                    Success = false
                });
            }
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if(!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return Ok(new RegistrationResponse()
                {
                    Errors = result.Errors.Select(e => e.Description).ToList(),
                    Success = false
                });
            }

            return Ok();
        }        
        
        [HttpGet]
        [Route("testemail/{email}")]
        public async Task<IActionResult> TestEmail(string email)
        {
            var emailBody = $"<h1>Test email sent to:</h1><h3>{email}</h3>";
            var successfulEmail = await SendEmail(email, "Test Subject", emailBody);
            if (!successfulEmail)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(email);
        }

        private async Task<bool> SendEmail(string email, string subject, string htmlBody)
        {
            try
            {
                var emailConfigOptions = Configuration.GetSection("EmailConfiguration");
                var fromEmail = emailConfigOptions.GetSection("From").Value;
                var username = emailConfigOptions.GetSection("EmailUsername").Value;
                var password = emailConfigOptions.GetSection("EmailPassword").Value;
                var host = emailConfigOptions.GetSection("SmtpServer").Value;
                var port = emailConfigOptions.GetSection("Port").Value;
            
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Woodcut Admin", 
                    fromEmail);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("Matt", 
                    email);
                message.To.Add(to);

                message.Subject = subject;
            
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;
                message.Body = bodyBuilder.ToMessageBody();
                SmtpClient client = new SmtpClient();
                await client.ConnectAsync(host, Int32.Parse(port), true);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                   new Claim("Id", user.Id),
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                   new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                   new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}