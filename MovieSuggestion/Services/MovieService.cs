using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieSuggestion.Models;
using MovieSuggestion.Models.Entities;
using MovieSuggestion.Models.Entities.View;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieSuggestion.Services
{
    public interface IMovieService
    {
        Task<MovieVM> CheckAndUpdateMovieList();
    }

    public class MovieService : IMovieService
    {
        public IConfiguration _config { get; }
        private readonly ApplicationDbContext _db;

        public MovieService(IConfiguration config, ApplicationDbContext db)
        {
            _config = config;
            _db = db;
        }

        public async Task<MovieVM> CheckAndUpdateMovieList()
        {
            var _lastMovieData = _db.Movie.Count() > 0 ? _db.Movie.OrderByDescending(x => x.PageNumber).FirstOrDefault() : null;
            int pageNumber = _lastMovieData != null ? (_lastMovieData.PageNumber + 1) : 1;

            MovieVM movieList = new MovieVM();

            movieList = await CheckMovieList(pageNumber);

            if (movieList.results.Any())
            {
                List<Movie> movieModel = new List<Movie>();

                var posterRootPath = "https://image.tmdb.org/t/p/original";

                movieList.results.ForEach(item =>
                {
                    string movieName = item.name ?? item.title;
                    if (!_db.Movie.Any(x => x.Name == movieName)) 
                    {
                        movieModel.Add(new Movie()
                        {
                            Name = movieName,
                            Image = posterRootPath + item.poster_path,
                            PageNumber = movieList.page
                        });
                    }
                });

                await _db.Movie.AddRangeAsync(movieModel);
                await _db.SaveChangesAsync();
            }

            return movieList;
        }

        private async Task<MovieVM> CheckMovieList(int pageNumber)
        {
            string apiKey = _config.GetValue<string>("TmdbConfig:api-key");
            string serviceUrl = "https://api.themoviedb.org/3/trending/all/day?api_key=" + apiKey + "&language=tr-TR&page=" + pageNumber.ToString();

            MovieVM movieList = new MovieVM();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(serviceUrl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    movieList = JsonConvert.DeserializeObject<MovieVM>(apiResponse);
                }
            }

            return movieList;
        }
    }
}
