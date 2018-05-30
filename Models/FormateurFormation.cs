using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class FormateurFormation
    {
        public Guid FormateurId { get; set; }

        [JsonIgnore]
        virtual public Formateur Formateur { get; set; }

        public Guid FormationId { get; set; }

        virtual public Formation Formation { get; set; }
    }
}
