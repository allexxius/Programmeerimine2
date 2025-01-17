using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Visit
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public IdentityUser User { get; set; }
        public string UserId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Duration { get; set; }
    }
}
