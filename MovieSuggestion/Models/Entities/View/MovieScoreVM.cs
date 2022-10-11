using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Models.Entities.View
{
    public class MovieScoreGetModel
    {
        public int MovieId { get; set; }

        public string MovieName { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int Rate { get; set; }

        public int AvgRate { get; set; }

        public string Note { get; set; }
    }

    public class MovieScorePostModel
    {
        public int MovieId { get; set; }

        public int UserId { get; set; }

        public int Rate { get; set; }

        [StringLength(250)]
        public string Note { get; set; }
    }
}
