using System.ComponentModel.DataAnnotations;

namespace ChatterBox.Interfaces.Entities
{
    public class ChatUser
    {
        [Required]
        public string Email { get; set; }

        public DateTime DateRegister { get; set; }

        public DateTime DateBirth { get; set; }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nickname { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
