using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuotePDFService.Models;

namespace QuotePDFService.Dtos
{
    public class ReadQuotePDFDTO
    {
        public int Id { get; set; }

        public Project Project { get; set; }

        public ICollection<TodoTemplate> TodoTemplates { get; set; } = new List<TodoTemplate>();
    }
}