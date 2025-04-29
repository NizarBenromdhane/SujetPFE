using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SujetPFE.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int ForumId { get; set; } // Clé étrangère vers Forum
        public Forum Forum { get; set; } // Propriété de navigation vers le forum
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
