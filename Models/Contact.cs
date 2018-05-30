using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Contact
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

        [Required(ErrorMessage = "Please provide a noTelephone")]
        public string NoTelephone1 { get; set; }

        public string NoTelephone2 { get; set; }

        public string Email { get; set; }

        public string Commentaire { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

        [JsonIgnore]
        virtual public ICollection<ContactEntreprise> ContactEntreprises { get; set; }
    }
}
