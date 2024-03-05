using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webapi.Data.Entities;
using WebAPICore.Dtos;
using WebAPICore.Interfaces;
using WebAPICore.Model;
namespace WebAPICore.Data.Repo
{
    public class UserRepositry : GenericRepository<User>, IUserRepositry
    {
        private readonly ReactJSDemoContext context;

        public UserRepositry(ReactJSDemoContext context) : base(context)
        {
            this.context = context;

        }

        public async Task<LoginResDto> login(string userName, string password)
        {

            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == userName);
            if (user == null || user.PasswordKey == null)
                return null;

            if (!MatchPasswordHash(password, user.Password, user.PasswordKey))
                return null;

            //    // return user;
            //      ApiError apiError = new ApiError();

            //     if (user == null)
            //     {
            //         apiError.ErrorCode = Unauthorized().StatusCode;
            //         apiError.ErrorMessage = "Invalid user name or password";
            //         apiError.ErrorDetails = "This error appear when provided user id or password does not exists";
            //         return Unauthorized(apiError);
            //     }

            var loginRes = new LoginResDto();
         
            loginRes.Token = CreateJWT(user);
          //  loginRes.RefreshToken = GenerateToken(user.Username);
            return loginRes;


        }

        private object Unauthorized()
        {
            throw new NotImplementedException();
        }

        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i])
                        return false;
                }

                return true;


            }
        }
        public void Register(RegistrationDto loginReqDto)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginReqDto.Password));

            }

            User user = new User();
            user.Username = loginReqDto.Username;
            user.email = loginReqDto.email;
            user.gender = loginReqDto.gender;
            user.role = loginReqDto.role;
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;

            user.ModifiedBy = "1";
            user.CreatedDate = DateTime.Now;
            this.context.Users.Add(user);
        }

        public async Task<bool> UserAlreadyExists(string userName)
        {
            return await context.Users.AnyAsync(x => x.Username == userName);
        }
        public async Task<IEnumerable<User>> GetUserslist()
        {
            return await context.Users.ToListAsync();
        }
        public async Task<User> FindUser(string userName)
        {
            return await FindAsync(b => b.Username == userName);

            //  return await context.Users.FirstOrDefaultAsync(x => x.Username == userName);
        }


        public async Task<User> Updateuser(string Username, RegistrationDto loginReqdto)
        {

            // if (Username != loginReqdto.Username)
            //   return "Update not allowed";

            var userFromDb = await FindAsync(p => p.Username == Username);

            //     if (userFromDb == null)
            //     {
            //  //       return "Update not allowed";
            //     }
            userFromDb.role = loginReqdto.role;
            userFromDb.isactive = loginReqdto.isactive;
            // object value = mapper.Map<RegistrationDto>(await FindAsync(a=>a.Username==Username);
            // await SaveAsync();
            // StatusCode(200);
            // await SaveAsync();
            return await FindAsync(b => b.Username == Username);

            //  return await context.Users.FirstOrDefaultAsync(x => x.Username == userName);
        }

        public void DeleteUser(string userName)
        {
            var user = context.Users.FirstOrDefault(x => x.Username == userName);
            context.Users.Remove(user);
        }

        //public string GenerateToken(string username)
        //{
        //    var randomnumber = new byte[32];
        //    using (var randomnumbergenerator = RandomNumberGenerator.Create())
        //    {
        //        randomnumbergenerator.GetBytes(randomnumber);
        //        string RefreshToken = Convert.ToBase64String(randomnumber);
        //        if (username != null)
        //        {
        //            var _user = context.RefreshtokenTables.FirstOrDefault(x => x.UserId == username);
        //            if (_user != null)
        //            {
        //                _user.RefreshToken = RefreshToken;
        //                context.SaveChanges();
        //            }
        //            else
        //            {
        //                RefreshtokenTable RefreshtokenTable = new RefreshtokenTable()
        //                {
        //                    UserId = username,
        //                    TokenId = new Random().Next().ToString(),
        //                    RefreshToken = RefreshToken,
        //                    isactive = true,
        //                    ModifiedBy = username,
        //                    CreatedDate = DateTime.Now,
        //                    IsDeleted = false

        //                };
        //                context.RefreshtokenTables.Add(RefreshtokenTable);
        //                context.SaveChanges();
        //            }
        //        }
        //        return RefreshToken;
        //    }
        //}
        //public LoginResDto Authenticate(string username, Claim[] claims)
        //{
        //    LoginResDto tokenResponse = new LoginResDto();
        //    var tokenkey = Encoding.ASCII.GetBytes("superSecretKey@345");
        //    var tokenhandler = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddSeconds(120),
        //         signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)

        //        );
        //    tokenResponse.Token = new JwtSecurityTokenHandler().WriteToken(tokenhandler);
        //    tokenResponse.RefreshToken = GenerateToken(username);


        //    return tokenResponse;
        //}
        //public LoginResDto Refresh(LoginResDto token)

        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token.Token);
        //    var username = securityToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

        //    // var username = principal.Identity.Name;
          
        //       //  var _reftable = context.RefreshtokenTables.FirstOrDefault(x => x.UserId == username);
        //   var _reftable = context.RefreshtokenTables.FirstOrDefault(o => o.UserId == username.ToString() && o.RefreshToken == token.RefreshToken);
        //    if (_reftable == null)
        //    {
        //        // return Unauthorized();
        //    }
        
        //    LoginResDto _result = Authenticate(username, securityToken.Claims.ToArray());
        //    return _result;
        //}
        private string CreateJWT(User user)
        {
            var key = Encoding.ASCII.GetBytes("superSecretKey@345");

            var claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier,user.email.ToString()),
                 new Claim(ClaimTypes.Role,user.role.ToString())
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(120),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }


    }
}