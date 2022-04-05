using CourseSubscription.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSubscription.Entity.Model
{
    public class SUBSCRIPTION : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public decimal SUB_AUTO_KEY { get; set; }
        public decimal USR_AUTO_KEY { get; set; } // we will provide from third-party service
        public decimal TRA_AUTO_KEY { get; set; }

        [ForeignKey("TRA_AUTO_KEY")]
        public virtual TRAINING TRAINING { get; set; }
    }
}
