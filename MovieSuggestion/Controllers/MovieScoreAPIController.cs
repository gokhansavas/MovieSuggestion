using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieSuggestion.Models;
using MovieSuggestion.Models.Entities;
using MovieSuggestion.Models.Entities.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieScoreAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public MovieScoreAPIController(ApplicationDbContext db,
                                       IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public IQueryable<MovieScoreGetModel> GetMovieScore()
        {
            var _data = _mapper.ProjectTo<MovieScoreGetModel>(_db.MovieScore.AsNoTracking());

            return _data;
        }

        [HttpGet("GetMovieScore/{movieId}/{userId}")]
        public async Task<IActionResult> GetMovieScore([FromRoute] int movieId, [FromRoute] int userId)
        {
            var _data = await _mapper.ProjectTo<MovieScoreGetModel>(_db.MovieScore.AsNoTracking().Where(x => x.MovieId == movieId && x.UserId == userId)).SingleOrDefaultAsync();

            return Ok(_data);
        }

        [HttpPost]
        public async Task<IActionResult> PostMovieScore([FromForm] MovieScorePostModel model)
        {
            if (model.Rate < 1 || model.Rate > 10)
                return BadRequest("1 - 10 arasında bir değer girmelisiniz!");

            if (string.IsNullOrEmpty(model.Note))
                return BadRequest("Not girmek zorundasınız!");

            if (await CheckMovieScoreRateAndNoteExists(model))
                return BadRequest("Bu filme daha önceden puan ve not eklediniz!");

            var _data = _mapper.Map<MovieScore>(model);

            _db.MovieScore.Add(_data);
            await _db.SaveChangesAsync();

            return Ok(_data);
        }

        private async Task<bool> CheckMovieScoreRateAndNoteExists(MovieScorePostModel model)
        {
            // Puan ve Not eklenmiş ise
            if (await _db.MovieScore.AsNoTracking().Where(ww => ww.MovieId == model.MovieId && ww.UserId == model.UserId && ww.Rate != 0 && !string.IsNullOrEmpty(ww.Note)).AnyAsync())
                return true;

            return false;
        }
    }
}
