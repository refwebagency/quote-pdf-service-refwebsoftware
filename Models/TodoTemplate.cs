using System.ComponentModel.DataAnnotations;

namespace QuotePDFService.Models
{
    public class TodoTemplate
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int ExternalToDoId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Experience { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Time { get; set; }

        [Required]
        public int SpecializationId  { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int ProjectTypeId { get; set; }
    }
}