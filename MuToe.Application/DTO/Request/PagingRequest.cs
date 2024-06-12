



using static MuTote.Application.Helpers.SortType;

namespace MuTote.Application.DTO.Request
{
    public class PagingRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SortOrder SortType { get; set; }
        public string ColName { get; set; } = "Id";
    }
}