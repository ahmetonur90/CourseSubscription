namespace CourseSubscription.Core.DTOs
{
    public class SubscriptionWithUserDto
    {
        public decimal SUB_AUTO_KEY { get; set; }
        public decimal USR_AUTO_KEY { get; set; }
        public decimal TRA_AUTO_KEY { get; set; }
        public string USER_NAME { get; set; }
        public string USER_GENDER { get; set; }
        public string USER_EMAIL { get; set; }
    }
}
