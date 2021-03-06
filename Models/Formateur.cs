using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Formateur
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please provide a nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Please provide a prenom")]
        public string Prenom { get; set; }

        [NotMapped]
        public string NomComplet
        {
            get
            {
                return Prenom + " " + Nom;
            }
        }

        virtual public ICollection<FormateurFormation> FormateurFormations { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

    }
    
}

