using AutoMapper;
using MovieSuggestion.Models.Entities;
using MovieSuggestion.Models.Entities.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Config.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserPostModel, User>();
            CreateMap<User, UserGetModel>();
        }
    }
}
