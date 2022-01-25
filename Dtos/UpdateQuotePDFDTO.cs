using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuotePDFService.Models;

namespace QuotePDFService.Dtos
{
    public class UpdateQuotePDFDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Project Project { get; set; }

        [Required]
        public ICollection<TodoTemplate> TodoTemplates { get; set; } = new List<TodoTemplate>();
    }
}