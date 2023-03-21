using System.ComponentModel.DataAnnotations;

namespace ChatterBox.Interfaces.Entities
{
    public class Message
    {
        public DateTime DateSent { get; set; }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
