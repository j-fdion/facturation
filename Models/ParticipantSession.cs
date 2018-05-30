using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class ParticipantSession
    {
        public Guid ParticipantId { get; set; }

        public Participant Participant { get; set; }

        public Guid SessionId { get; set; }

        public Session Session { get; set; }
    }
}
