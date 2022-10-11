using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Models.Entities.View
{
    public class MovieVM
    {
        public int page { get; set; }

        public List<ResultList> results { get; set; }
    }

    public class ResultList
    {
        public string name { get; set; }

        public string title { get; set; }

        public string poster_path { get; set; }
    }

    public class MovieGetModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public int AvgRate { get; set; }
    }
}
