using System.ComponentModel.DataAnnotations;

namespace TestOgSikkerhed.Data
{
    public class Cpr
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string User { get; set; } = string.Empty;

        [Required]
        public string CprNumber { get; set; } = string.Empty;

        [Required]
        public string Salt { get; set; } = string.Empty;

        public ICollection<ToDo> ToDoItems { get; set; } = new List<ToDo>();
    }
}
