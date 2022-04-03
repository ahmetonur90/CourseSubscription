namespace CourseSubscription.Core.DTOs
{
    public class TrainingFilterDto : PagingParamsDto
    {
        public string Month { get; set; }
        public string TrainingCode { get; set; }
        public string TrainingName { get; set; }
    }
}
