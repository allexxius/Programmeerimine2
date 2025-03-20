using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Time : Entity
    {

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeOnly VisitTime {  get; set; }

        [Required]
        public Boolean Free { get; set; }

        [Required]
        public int DoctorId { get; set; }
    }
}
