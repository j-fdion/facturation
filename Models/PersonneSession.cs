using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class PersonneSession
    {
        public Guid PersonneId { get; set; }

        public Personne Personne { get; set; }

        public Guid SessionId { get; set; }

        public Session Session { get; set; }
    }
}
