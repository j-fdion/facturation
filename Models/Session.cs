using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Session
    {
        [Required(ErrorMessage = "Please provide an id")]
        public Guid Id { get; set; }

        public Guid? EntrepriseId { get; set; }
        [Required(ErrorMessage = "Please provide a entreprise")]
        public Entreprise Entreprise { get; set; }

        public Guid? ContactId { get; set; }
        [Required(ErrorMessage = "Please provide a contact")]
        public Contact Contact { get; set; }

        public Guid? FormationId { get; set; }
        [Required(ErrorMessage = "Please provide a formation")]
        public Formation Formation  { get; set; }

        public Guid? FormateurId { get; set; }
        [Required(ErrorMessage = "Please provide a formateur")]
        public Formateur Formateur { get; set; }

        public Guid? SalleId { get; set; }
        [Required(ErrorMessage = "Please provide a salle")]
        public Salle Salle { get; set; }

        [Required(ErrorMessage = "Please provide a date")]
        public DateTimeOffset? Date { get; set; }

        public string BonCommande { get; set; }

        [Required(ErrorMessage = "Please provide a typeFacturation")]
        public string TypeFacturation { get; set; }

        [Required(ErrorMessage = "Please provide a utiliseDureeSession")]
        public bool UtiliseDureeSession { get; set; }

        [Required(ErrorMessage = "Please provide a duree")]
        public float Duree { get; set; }

        [Required(ErrorMessage = "Please provide a utilisePrixSession")]
        public bool UtilisePrixSession { get; set; }

        [Required(ErrorMessage = "Please provide a Prix")]
        public float Prix { get; set; }

        [Required(ErrorMessage = "Please provide a NombrePersonnes")]
        public float NombrePersonnesAttendues { get; set; }

        [JsonIgnore]
        public ICollection<PersonneSession> PersonneSessions { get; set; }

        [JsonIgnore]
        public ICollection<FraisSupplementaireSession> FraisSupplementaireSessions { get; set; }

        [JsonIgnore]
        public ICollection<ParticipantSession> ParticipantSessions { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

    }
    
}

