using System.ComponentModel.DataAnnotations;

namespace CourseSubscription.Entity.Model
{
    public class Sp_UserSubscription
    {
        [Key]
        public decimal SUB_AUTO_KEY { get; set; }
        public decimal USR_AUTO_KEY { get; set; }
        public decimal TRA_AUTO_KEY { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public string MONTH { get; set; }
    }
}
