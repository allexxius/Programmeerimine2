using System;
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
        public bool Paid { get; set; }  // Use 'bool' instead of 'Boolean'

        [Required]
        public int VisitId { get; set; }
    }
}
