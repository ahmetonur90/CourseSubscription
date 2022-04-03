namespace CourseSubscription.Core.DTOs
{
    public class PagingParamsDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PagingParamsDto()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public PagingParamsDto(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
