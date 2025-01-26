using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Time
    {
        internal static int id;

        [Required]
        public int Id { get; set; }

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
