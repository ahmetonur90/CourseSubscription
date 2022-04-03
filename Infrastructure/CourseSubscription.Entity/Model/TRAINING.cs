using CourseSubscription.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSubscription.Entity.Model
{
    public class TRAINING : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public decimal TRA_AUTO_KEY { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public decimal COU_AUTO_KEY { get; set; }
        public string MONTH { get; set; }
        public string STATUS { get; set; }

    }
}
