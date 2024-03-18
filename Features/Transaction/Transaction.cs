using System.ComponentModel.DataAnnotations;

namespace AksiaAssessment2024.Features.Transaction
{
    public class Transaction
    {
        [Key]
        public Guid? Id { get; set; }

        [Required]
        [StringLength(200)]
        public string ApplicationName { get; set; } = "";

        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [StringLength(300)]
        public string Filename { get; set; } = "";

        [Url]
        public string Url { get; set; } = "";

        [Required]
        public DateTime Inception { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Range(0.0, 100.0)]
        public decimal? Allocation { get; set; }

    }
}
   