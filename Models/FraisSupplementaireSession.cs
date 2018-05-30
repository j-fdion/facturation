using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class FraisSupplementaireSession
    {
        public Guid SessionId { get; set; }

        public Session Session { get; set; }

        public Guid FraisSupplementaireId { get; set; }

        public FraisSupplementaire FraisSupplementaire { get; set; }

        public float Quantite { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }
    }
}
