using AutoMapper;
using TimeBasedOTPBT.Models.Users;
using TimeBasedOTPBT.Persistence.Entities;

namespace TimeBasedOTPBT.Common.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}
