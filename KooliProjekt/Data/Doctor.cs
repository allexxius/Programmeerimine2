using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Doctor : Entity
    {

        [Required]
        [StringLength(50)]
        public string Specialization { get; set; }
        
        [StringLength(50)]
        public string Name { get; set; }


        [Required]
        public int UserId { get; set; }


    }
}
