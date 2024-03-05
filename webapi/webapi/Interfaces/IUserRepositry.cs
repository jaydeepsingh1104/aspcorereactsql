using System.Security.Claims;
using WebAPICore.Dtos;
using WebAPICore.Model;

namespace WebAPICore.Interfaces
{
    public interface IUserRepositry : IGenericRepository<User>
    {
        Task<LoginResDto> login(string userName, string password);
        void Register(RegistrationDto LoginReqDto);
        Task<bool> UserAlreadyExists(string userName);
        Task<IEnumerable<User>> GetUserslist();
        Task<User> FindUser(string userName);
        Task<User> Updateuser(string Username, RegistrationDto loginReqdto);
        void DeleteUser(string userName);
        //LoginResDto Refresh(LoginResDto token);
        //LoginResDto Authenticate(string username, Claim[] claims);
        //string GenerateToken(string username);

    }
}