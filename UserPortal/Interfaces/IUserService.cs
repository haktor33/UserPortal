using System.Threading.Tasks;
using UserPortal.Entities;
using UserPortal.Models;

namespace UserPortal.Interfaces
{
    public interface IUserService
    {
        public Task<string> Authenticate(AuthenticateRequest model);
        public Task<User> Registration(RegistrationRequest model);
        public Task<User> UpdateCurrentUserDataAsync(UserUpdateRequest model);
        public Task<User> GetById(long userId);
    }
}
