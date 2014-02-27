using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MongoDB.Driver;

namespace PagedList
{

    /// <summary>
    /// Paginate MongoCursor without mapping
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class MongoPagedList<T> : BasePagedList<T>
    {
        /// <summary>
        /// Paginate mongo cursor
        /// </summary>
        /// <param name="superset">MongoCursor</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MongoPagedList(MongoCursor<T> superset, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");

            // set source to blank list if superset is null to prevent exceptions
            TotalItemCount = superset == null ? 0 : unchecked((int) superset.Count());
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = TotalItemCount > 0
                ? (int) Math.Ceiling(TotalItemCount/(double) PageSize)
                : 0;
            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < PageCount;
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber >= PageCount;
            FirstItemOnPage = (PageNumber - 1)*PageSize + 1;
            var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
            LastItemOnPage = numberOfLastItemOnPage > TotalItemCount
                ? TotalItemCount
                : numberOfLastItemOnPage;

            // add items to internal list
            if (superset != null && TotalItemCount > 0)
                Subset.AddRange(pageNumber == 1
                    ? superset.Skip(0).Take(pageSize).ToList()
                    : superset.Skip((pageNumber - 1)*pageSize).Take(pageSize).ToList()
                    );
        }
    }

    /// <summary>
    /// Paginate MongoCursor with Source -> Destination mapping (ex. Model to ViewModel mapping)
    /// </summary>
    /// <typeparam name="TSource">Source Type</typeparam>
    /// <typeparam name="TDestination">Destination Type</typeparam>
    public class MongoPagedList<TSource,TDestination> : BasePagedList<TDestination>
    {
        /// <summary>
        /// Paginate MongoCursor
        /// </summary>
        /// <param name="superset">MongoCursor</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public MongoPagedList(MongoCursor<TSource> superset, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");

            // set source to blank list if superset is null to prevent exceptions
            TotalItemCount = superset == null ? 0 : unchecked((int) superset.Count());
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = TotalItemCount > 0
                ? (int) Math.Ceiling(TotalItemCount/(double) PageSize)
                : 0;
            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < PageCount;
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber >= PageCount;
            FirstItemOnPage = (PageNumber - 1)*PageSize + 1;
            var numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
            LastItemOnPage = numberOfLastItemOnPage > TotalItemCount
                ? TotalItemCount
                : numberOfLastItemOnPage;

            // add items to internal list
            if (superset == null) return;
            var res = pageNumber == 1
                ? Mapper.Map<List<TSource>, List<TDestination>>(superset.Skip(0).Take(pageSize).ToList())
                : Mapper.Map<List<TSource>, List<TDestination>>(superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
            if (TotalItemCount > 0)
                Subset.AddRange(res);
        }
	}
}