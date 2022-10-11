using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Services
{
    public class Scheduler : IJob
    {
        private IMovieService _movieService;

        public Scheduler(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //await _movieService.CheckAndUpdateMovieList();
        }
    }
}
