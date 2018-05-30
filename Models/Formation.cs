using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Formation
    {

        [Required(ErrorMessage = "Please provide an Id")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please provide a Titre")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "Please provide a Duree")]
        public float Duree { get; set; }

        [Required(ErrorMessage = "Please provide a TauxHoraire")]
        public float TauxHoraire { get; set; }

        [Required(ErrorMessage = "Please provide a PrixForfaitaire")]
        public float PrixForfaitaire { get; set; }

        [Required(ErrorMessage = "Please provide a PrixUnitaire")]
        public float PrixUnitaire { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

        [JsonIgnore]
        virtual public ICollection<FormateurFormation> FormateurFormations { get; set; }
    }
    
}

