using EInsurance_App.ViewModels.Common;

namespace EInsurance_App.Helpers
{
    public static class PaginationHelper
    {
        // For IQueryable (DB level pagination)
        public static PagedResult<T> ToPagedResult<T>(
            this IQueryable<T> query,
            int page,
            int pageSize)
        {
            if (page < 1) page = 1;

            var totalRecords = query.Count();

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                Data = data,
                PageNumber = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        // For IEnumerable (after AsEnumerable)
        public static PagedResult<T> ToPagedResult<T>(
            this IEnumerable<T> data,
            int page,
            int pageSize)
        {
            if (page < 1) page = 1;

            var totalRecords = data.Count();

            var result = data
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                Data = result,
                PageNumber = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}