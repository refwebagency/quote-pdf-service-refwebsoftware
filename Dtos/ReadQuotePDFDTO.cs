using System.ComponentModel.DataAnnotations;

namespace QuotePDFService.Dtos
{
    public class ReadQuotePDFDTO
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ProjectId { get; set; }
    }
}