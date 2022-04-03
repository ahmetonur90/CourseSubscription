using CourseSubscription.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseSubscription.Entity.Model
{
    public class COURSE : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public decimal COU_AUTO_KEY { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public string TITLE { get; set; }
        public string DESCRIPTION { get; set; }
        public string STATUS { get; set; }
    }
}
