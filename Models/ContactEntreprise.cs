using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class ContactEntreprise
    {
        public Guid ContactId { get; set; }

        virtual public Contact Contact { get; set; }

        public Guid EntrepriseId { get; set; }

        [JsonIgnore]
        virtual public Entreprise Entreprise { get; set; }
    }
}
