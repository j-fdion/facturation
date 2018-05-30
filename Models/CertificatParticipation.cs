using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class CerticatParticipation
    {
        public Personne personne { get; set; }

        public Session session { get; set; }
    }
}
