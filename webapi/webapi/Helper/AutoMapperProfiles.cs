using AutoMapper;
using WebAPICore.Model;
using WebAPICore.Dtos;
using Microsoft.Extensions.Logging;

namespace WebAPICore.Helper
{
    public class AutoMapperProfiles: Profile
    {

        public AutoMapperProfiles()
        {
   
           CreateMap<User, LoginReqDto>().ReverseMap(); 
            CreateMap<User, LoginResDto>().ReverseMap(); 
             CreateMap<User, RegistrationDto>().ReverseMap(); 
             
        }
        
    }
}