using System;

namespace DevPodcasts.ViewModels
{
    public class PaginationInfo
    {
        public PaginationInfo(int pageIndex, int itemsPerPage, int totalItems)
        {
            ActualPage = pageIndex;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = int.Parse(Math.Ceiling((decimal) totalItems / itemsPerPage).ToString());
            Next = ActualPage == TotalPages - 1 ? "is-disabled" : "";
            Previous = ActualPage == 0 ? "is-disabled" : "";
        }

        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int ActualPage { get; set; }
        public int TotalPages { get; set; }
        public string Previous { get; set; }
        public string Next { get; set; }
    }
}
