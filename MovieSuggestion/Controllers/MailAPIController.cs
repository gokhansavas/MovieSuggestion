using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSuggestion.Models;
using MovieSuggestion.Models.Entities.View;
using MovieSuggestion.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MailAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private IMailService _mailService;

        public MailAPIController(ApplicationDbContext db,
                                 IMailService mailService)
        {
            _db = db;
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<bool> Send([FromForm] MailVM mailModel)
        {
            var result = await _mailService.SendAsync(mailModel);

            return result;
        }
    }
}
