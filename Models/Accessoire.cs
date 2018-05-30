using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Accessoire
    {
        [Required(ErrorMessage = "Please provide an id")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please provide a modele")]
        public string Modele { get; set; }

        [Required(ErrorMessage = "Please provide a modele")]
        public float Prix { get; set; }

        [Required(ErrorMessage = "Please provide a type")]
        public virtual TypeAccessoire TypeAccessoire { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }
    }
}
