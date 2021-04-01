using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CookbookAPI.DTOs;
using CookbookAPI.Extensions;
using CookbookAPI.Requests;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;

namespace CookbookAPI.ViewModels
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public List<T> Items { get; }


        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }



        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize,
             string sortBy, string sortDirection)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (!string.IsNullOrEmpty(sortDirection) && sortDirection.ToLower() == SortDirection.DESC)
                    source = source.OrderByDescending(sortBy);
                else
                    source = source.OrderBy(sortBy);
            }

            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
