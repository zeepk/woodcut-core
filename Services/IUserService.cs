using System.Threading.Tasks;

namespace dotnet5_webapp.Services
{
    public interface IUserService
    {
        Task<string> TestFunction();

    }
}