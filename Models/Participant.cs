using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Participant
    {
        [Required(ErrorMessage = "Please provide an id")]
        public Guid Id { get; set; }

        public virtual Personne Personne  { get; set; }

        [Required(ErrorMessage = "Please provide an absence")]
        public bool Absence { get; set; }

        public virtual Accessoire Accessoire { get; set; }

        public string Grandeur { get; set; }

        public bool AccessoireSeulement { get; set; }

        [JsonIgnore]
        public ICollection<ParticipantSession> ParticipantSessions { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

        [NotMapped]
        public Guid SessionId { get; set; }

    }
}

