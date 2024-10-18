using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Document
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string File { get; set; }

        [Required]
        public int Visit { get; set; }
    }
}
