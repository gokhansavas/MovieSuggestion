using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Models.Entities.View
{
    public class MailVM
    {
        public int MovieId { get; set; }

        public string ToAddressName { get; set; }

        public string ToAddress { get; set; }
    }
}
