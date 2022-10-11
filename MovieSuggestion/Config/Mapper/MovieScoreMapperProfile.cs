using AutoMapper;
using MovieSuggestion.Models.Entities;
using MovieSuggestion.Models.Entities.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Config.Mapper
{
    public class MovieScoreMapperProfile : Profile
    {
        public MovieScoreMapperProfile()
        {
            CreateMap<MovieScorePostModel, MovieScore>();
            CreateMap<MovieScore, MovieScoreGetModel>()
                .ForMember(d => d.MovieName, map => map.MapFrom(mf => mf.Movie.Name))
                .ForMember(d => d.UserName, map => map.MapFrom(mf => mf.User.FullName))
                .ForMember(d => d.AvgRate, map => map.MapFrom(mf => mf.Movie.MovieScores.Any(x => x.MovieId == mf.Movie.Id) ? Convert.ToInt32(mf.Movie.MovieScores.Where(x => x.MovieId == mf.Movie.Id).Average(x => x.Rate)) : 0));
        }
    }
}
