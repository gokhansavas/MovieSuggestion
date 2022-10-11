using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieSuggestion.Models;
using MovieSuggestion.Models.Entities;
using MovieSuggestion.Models.Entities.View;
using MovieSuggestion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieSuggestion.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private IMovieService _movieService;
        private readonly IMapper _mapper;

        public MovieAPIController(ApplicationDbContext db, 
                                  IMovieService movieService,
                                  IMapper mapper)
        {
            _db = db;
            _movieService = movieService;
            _mapper = mapper;
        }

        [HttpGet]
        public IQueryable<MovieGetModel> GetMovie([FromQuery] bool random = false, [FromQuery] PaginationParameters @params = null)
        {
            var _data = _mapper.ProjectTo<MovieGetModel>(_db.Movie.AsNoTracking()).ToList();

            var paginationMetadata = new PaginationMetadata(_data.Count(), @params.Page, @params.ItemsPerPage);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            _data = _data.Skip((@params.Page - 1) * @params.ItemsPerPage)
                         .Take(@params.ItemsPerPage)
                         .ToList();

            if (random)
            {
                _data = _data.OrderBy(r => Guid.NewGuid()).Take(6).ToList();
            }

            return _data.AsQueryable();
        }

        [HttpGet("GetMovie/{id}")]
        public async Task<IActionResult> GetMovie([FromRoute] int id)
        {
            var _data = await _mapper.ProjectTo<MovieGetModel>(_db.Movie.AsNoTracking().Where(x => x.Id == id)).SingleOrDefaultAsync();

            return Ok(_data);
        }

        // API üzerinden kayıt tetiklemek için (Job çalıştığından bu metoda ihtiyaç yok)
        [HttpGet("CheckAndUpdateMovieList")]
        public async Task<IActionResult> CheckAndUpdateMovieList()
        {
            var _data = await _movieService.CheckAndUpdateMovieList();

            return Ok(_data);
        }
    }
}
