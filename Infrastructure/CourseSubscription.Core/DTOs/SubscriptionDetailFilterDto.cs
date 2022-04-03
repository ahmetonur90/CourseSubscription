namespace CourseSubscription.Core.DTOs
{
    public class SubscriptionDetailFilterDto : SubscriptionFilterDto
    {
        public string CourseName { get; set; }
        public string UserName { get; set; }
    }
}
