using System.ComponentModel.DataAnnotations;

namespace QuotePDFService.Dtos
{
    public class CreateQuotePDFDTO
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public int ProjectId { get; set; }
    }
}