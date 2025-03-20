using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class InvoiceLine : Entity
    {

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Service {  get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
