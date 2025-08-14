namespace WebApplication1.DTOs
{
    public class PaginationParamsDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        private const int maxPageSize = 50;

        public int ValidatedPageSize
        {
            get => (PageSize > maxPageSize) ? maxPageSize : PageSize;
        }
    }
}
