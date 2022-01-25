using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuotePDFService.Models
{
    public class QuotePDF
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Project Project { get; set; }

        [Required]
        public ICollection<TodoTemplate> TodoTemplates { get; set; } = new List<TodoTemplate>();
    }
}