using System.ComponentModel.DataAnnotations;

namespace ChatterBox.Interfaces.Entities
{
    public class Group
    {
        public DateTime DateCreated { get; set; }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int OwnerId { get; set; }
    }
}
