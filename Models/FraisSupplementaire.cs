using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class FraisSupplementaire
    {
        [Required(ErrorMessage = "Please provide an id")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please provide a nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Please provide a prix")]
        public float CoutUnite { get; set; }

        public string NomUnite { get; set; }

        public string NomQuantite { get; set; }

        [JsonIgnore]
        public List<FraisSupplementaireSession> FraisSupplementaireSessions { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }
    }
}
