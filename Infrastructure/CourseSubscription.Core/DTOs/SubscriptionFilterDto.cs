namespace CourseSubscription.Core.DTOs
{
    public class SubscriptionFilterDto : PagingParamsDto
    {
        public string Month { get; set; }
        public string TrainingName { get; set; }
    }
}
