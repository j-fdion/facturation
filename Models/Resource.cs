using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppFactu.Models
{
    public class Resource
    {
        public Guid Id { get; set; }

        public string Salle { get; set; }

        public string Title { get; set; }

    }
    
}

