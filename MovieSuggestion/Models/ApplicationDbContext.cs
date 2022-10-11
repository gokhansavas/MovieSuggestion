using Microsoft.EntityFrameworkCore;
using MovieSuggestion.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        #region User
        public DbSet<User> User { get; set; }
        #endregion

        #region Movie
        public DbSet<Movie> Movie { get; set; }
        #endregion

        #region MovieScore
        public DbSet<MovieScore> MovieScore { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Movie Score
            builder.Entity<MovieScore>().HasKey(c => new { c.MovieId, c.UserId });
            #endregion
        }
    }
}
