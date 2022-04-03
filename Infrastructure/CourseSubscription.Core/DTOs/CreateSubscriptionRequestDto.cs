using System.ComponentModel.DataAnnotations;

namespace CourseSubscription.Core.DTOs
{
    public class CreateSubscriptionRequestDto
    {
        [Required]
        [Range(1, double.MaxValue)]
        public decimal USR_AUTO_KEY { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal TRA_AUTO_KEY { get; set; }
    }
}
