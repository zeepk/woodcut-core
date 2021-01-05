using System;
using System.Threading.Tasks;

namespace dotnet5_webapp.Services
{
    public class UserService : IUserService
    {
        public async Task<string> TestFunction()
        {
            await Task.Delay(1);
            var message = "This is a test!";
            Console.WriteLine(message);
            return message;
        }
    }
}