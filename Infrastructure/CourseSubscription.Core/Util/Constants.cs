namespace CourseSubscription.Core.Util
{
    public static class Constants
    {
        public const string Successful = "Successful..!";
        public const string Fail = "Fail..!";
        public const string InvalidRequest = "Invalid Request..!";
        public const string InvalidUsername = "Invalid Username..!";
        public const string InvalidPassword = "Invalid Password..!";
        public const string InvalidUser = "Invalid User..!";
    }

    public static class CourseConstants
    {
        public const string CourseNotFound = "No Course were found..!";
        public const string CourseIsNotActive = "Course is not active..!";
    }

    public static class TrainingConstants
    {
        public const string TrainingNotFound = "No Training were found..!";
        public const string TrainingIsNotActive = "Training is not active..!";
        public const string TrainingExceeded = "Only one training should be added to one course..!";
    }

    public static class SubscriptionConstants
    {
        public const string UsersNotFound = "No User were found..!";
        public const string UserIsNotActive = "User is not active..!";
        public const string UserCanNotSubscribe = "User could not subscribe to more than one training in the same month..!";
        public const string SubscriptionsNotFound = "No subscriptions were found..!";
    }
}
