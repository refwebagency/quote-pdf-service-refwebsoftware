using System.ComponentModel.DataAnnotations;

namespace QuotePDFService.Models
{
    public class QuotePDF
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int ProjectId { get; set; }
    }
}