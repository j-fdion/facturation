using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public ICollection<Guid> ResourceIds { get; set; }

        public string Title { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public string Color { get; set; }

    }
    
}

