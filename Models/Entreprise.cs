using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Entreprise
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please provide a nom")]
        public string Nom { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

        virtual public ICollection<ContactEntreprise> ContactEntreprises { get; set; }
    }
}
