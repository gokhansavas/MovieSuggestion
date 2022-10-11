using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Models.Entities
{
    public class MovieScore
    {
        [Key]
        public int MovieId { get; set; }

        [Key]
        public int UserId { get; set; }

        public int Rate { get; set; }

        [Required]
        [StringLength(250)]
        public string Note { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
