using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieSuggestion.Config;
using MovieSuggestion.Models;
using MovieSuggestion.Models.Entities;
using MovieSuggestion.Models.Entities.View;
using MovieSuggestion.Models.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserAPIController(ApplicationDbContext db,
                                 IConfiguration config,
                                 IMapper mapper)
        {
            _db = db;
            _config = config;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IQueryable<UserGetModel> GetUser()
        {
            var _data = _mapper.ProjectTo<UserGetModel>(_db.User.AsNoTracking());

            return _data;
        }

        [HttpPost("Register")]
        public async Task<bool> Register([FromForm] UserPostModel model)
        {
            if (await _db.User.AnyAsync(x => x.Email == model.Email))
            {
                return false;
            }

            var _data = _mapper.Map<User>(model);

            _db.User.Add(_data);
            await _db.SaveChangesAsync();
            return true;
        }

        [HttpPost("Login")]
        public async Task<Token> Login([FromForm] UserLoginModel userLogin)
        {
            User user = await _db.User.FirstOrDefaultAsync(x => x.Email == userLogin.Email && x.Password == userLogin.Password);
            if (user != null)
            {
                TokenHandler tokenHandler = new TokenHandler(_config);
                Token token = tokenHandler.CreateAccessToken(user);

                user.Token = token.RefreshToken;
                user.TokenEndDate = token.Expiration.AddMinutes(10);
                await _db.SaveChangesAsync();

                return token;
            }

            return null;
        }
    }
}
