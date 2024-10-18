using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Invoice
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Sum { get; set; }

        [Required]
        public Boolean Paid { get; set; }

        [Required]
        public int VisitId { get; set; }
    }
}
