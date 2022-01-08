using System.ComponentModel.DataAnnotations;

namespace QuotePDFService.Dtos
{
    public class UpdateQuotePDFDTO
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}