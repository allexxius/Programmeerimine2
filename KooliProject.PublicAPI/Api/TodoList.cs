using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.BlazorApp
{
    public class TodoList
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Title { get; set; }
    }
}
    