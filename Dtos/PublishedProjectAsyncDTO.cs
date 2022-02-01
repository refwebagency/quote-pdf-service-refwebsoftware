using System;
using System.ComponentModel.DataAnnotations;

namespace QuotePDFService.Dtos
{
    public class PublishedProjectAsyncDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }        

        [Required]
        public DateTime EndtDate { get; set; }

        [Required]
        public int ProjectTypeId { get; set; }

        [Required]
        public int ClientId { get; set; }

        public string Event { get; set; }
    }
}