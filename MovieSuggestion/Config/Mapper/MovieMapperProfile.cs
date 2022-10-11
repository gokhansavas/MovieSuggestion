using AutoMapper;
using MovieSuggestion.Models.Entities;
using MovieSuggestion.Models.Entities.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Config.Mapper
{
    public class MovieMapperProfile : Profile
    {
        public MovieMapperProfile()
        {
            CreateMap<Movie, MovieGetModel>()
                .ForMember(d => d.AvgRate, map => map.MapFrom(mf => mf.MovieScores.Any(x => x.MovieId == mf.Id) ? Convert.ToInt32(mf.MovieScores.Where(x => x.MovieId == mf.Id).Average(x => x.Rate)) : 0));
        }
    }
}
