using System.ComponentModel.DataAnnotations;

namespace CourseSubscription.Core.DTOs
{
    public class UpdateTrainingRequestDto
    {
        [Required]
        public decimal TRA_AUTO_KEY { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string NAME { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string CODE { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal COU_AUTO_KEY { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string MONTH { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string STATUS { get; set; }
    }
}
