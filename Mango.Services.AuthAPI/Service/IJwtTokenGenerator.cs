using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Service
{
    public interface IJwtTokenGenerator
    {
        string GenrateToken(ApplicationUsers users, IEnumerable<string> roles); 
    }
}
