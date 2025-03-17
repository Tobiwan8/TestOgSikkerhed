using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestOgSikkerhed.Data
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Cpr")]
        public int UserID { get; set; }

        [Required]
        public string Item { get; set; } = string.Empty;

        public Cpr? Cpr { get; set; }
    }
}
