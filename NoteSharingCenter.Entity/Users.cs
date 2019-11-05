using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Entity
{
    [Table("Users")]
    public class Users : MyEntityBase
    {
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(30)]
        public string Surname { get; set; }

        [Required, StringLength(30)]
        public string Username { get; set; }

        [Required, StringLength(50)]
        public string Email { get; set; }

        [Required, StringLength(30)]
        public string Password { get; set; }

        [StringLength(500)]
        public string AboutMe { get; set; }

        [StringLength(200)]
        public string ProfileImageFilename { get; set; }

        [Required]
        public Guid ActiveteGuid { get; set; }

        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
