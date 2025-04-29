using System.ComponentModel.DataAnnotations;

namespace SujetPFE.Domain.Entities
{
    public class Forum
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
        public int NumberOfReplies { get; set; }
        public DateTime? LastActivityDate { get; set; }

        // Collection de messages associés à ce forum
        public ICollection<Message> Messages { get; set; }
    }
}