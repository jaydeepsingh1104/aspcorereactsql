using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPICore.Dtos;
using WebAPICore.Interfaces;
using WebAPICore.Model;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using WebAPICore.Data;
using webapi.Data.Entities;

namespace WebAPICore.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class AccountController : ControllerBase
    {
        // private readonly IUnitOfWork iuow;
          private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        private readonly ReactJSDemoContext contetx;

        public AccountController(IUnitOfWork uow, IMapper mapper,ReactJSDemoContext contetx)
        {
            this.mapper = mapper;
            this.contetx = contetx;
            // this.iuow = iuow;
            this.uow = uow;

        }
    [AllowAnonymous]

        [HttpGet("Users")]
        public async Task<IActionResult> GetUserslist()
        {


            var registrationDto = mapper.Map<IEnumerable<RegistrationDto>>(  await uow.UserRepositry.GetUserslist());
            return Ok(registrationDto);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDto loginReq)
        {

            var result=await uow.UserRepositry.login(loginReq.Username, loginReq.Password);
            return Ok(result);

        }
   
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDto loginReq)
        {
             uow.UserRepositry.Register(loginReq);
            await uow.SaveAsync();
            return StatusCode(201);
        }
        [AllowAnonymous]
        [HttpGet("findUserbyuserName/{Username}")]
        public async Task<IActionResult> findUserByUsername(string Username)
        {
            return Ok(mapper.Map<RegistrationDto>(await uow.UserRepositry.FindUser(Username)));
        }
        [AllowAnonymous]
        [HttpPut("updateuser/{Username}")]
        public async Task<IActionResult> updateuser(string Username, RegistrationDto loginReqdto)
        {
           // uow.UserRepositry.Updateuser(Username, loginReqdto);
            if (Username != loginReqdto.Username)
                return BadRequest("Update not allowed");

            var userFromDb = await uow.UserRepositry.FindUser(Username);

            if (userFromDb == null)
            {
                return BadRequest("Update not allowed");
            }
            userFromDb.role = loginReqdto.role;
            userFromDb.isactive = loginReqdto.isactive;

            await uow.SaveAsync();
            return StatusCode(200);
        }
        [HttpDelete("deleteuser/{Username}")]
        public async Task<IActionResult> Delete(string Username)
        {
            uow.UserRepositry.DeleteUser(Username);
            await uow.SaveAsync();
            return Ok(Username);
        }

        [AllowAnonymous]
        [Route("Refresh")]
        [HttpPost]
        public async Task<IActionResult> Refresh(LoginResDto token)
        {

            return Ok(uow.UserRepositry.Refresh(token));

        }

        private string CreateJWT1(User user)
        {
       return "dfd";
        }

        [NonAction]
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