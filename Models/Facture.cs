using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Facture
    {
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumericId { get; set; }

        public Entreprise Entreprise  { get; set; }

        public ICollection<Session> Sessions { get; set; }

        public ICollection<PersonneSession> PersonneSessions { get; set; }

        public string BonCommande { get; set; }

        public string NoFacture { get; set; }

        public DateTime? Date { get; set; }
            
        public DateTime? PeriodeDebut { get; set; }

        public DateTime? PeriodeFin { get; set; }

        public DateTimeOffset? DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

    }
    
}

