using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Invoice : Entity
    {


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
