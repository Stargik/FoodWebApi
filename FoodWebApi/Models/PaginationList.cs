using System;
namespace FoodWebApi.Models
{
	public class PaginationList<T> : List<T>
	{
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PaginationList(List<T> items, int totalCount, int numberPage, int pageSize)
		{
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = numberPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            AddRange(items);
        }

        public static PaginationList<T> ToPaginationList(IEnumerable<T> source, int numberPage, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((numberPage - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationList<T>(items, count, numberPage, pageSize);
        }
    }
}

