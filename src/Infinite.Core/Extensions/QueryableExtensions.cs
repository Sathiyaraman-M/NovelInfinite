﻿using System.Linq.Expressions;
using Infinite.Core.Specifications.Base;
using Infinite.Shared.Contracts;
using Infinite.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Core.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
    {
        if (source == null) throw new Exception("IQueryable source for pagination is empty.");
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        var count = await source.CountAsync();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
    }

    public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class, IEntity
    {
        if (spec == null) 
            return query;
        
        if (spec.FilterCondition != null)
            return query.Where(spec.FilterCondition);

        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> query, bool condition, Expression<Func<TSource, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }
}