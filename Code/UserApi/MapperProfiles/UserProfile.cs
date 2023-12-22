using AutoMapper;
using BusinessLogic.Models;
using UserApi.DTOs;

namespace UserApi.MapperProfiles
{
    /// <summary>
    /// AutoMapper profile that allows generating 
    /// new instances of DataAccess.Entity.User or
    /// Models.User
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, DataAccess.Entities.User>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
